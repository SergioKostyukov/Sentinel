using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using SentinelApi.Monitoring.Application.Interfaces;
using SentinelApi.Monitoring.Application.Interfaces.Infrastructure;

namespace SentinelApi.Monitoring.Infrastructure.Background;

/// <summary>
/// Фоновий сервіс для запуску запланованих перевірок.
/// 1. Виконує перевірки щодня о 2:00 ранку.
/// 2. Використовує паралельне виконання для обробки кількох перевірок одночасно.
/// </summary>
public class CheckSchedulerBackgroundService(IServiceScopeFactory scopeFactory) : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly Logger _logger = LogManager.GetLogger("BackgroundService");

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                var now = DateTime.Now;

                // Опитуємо раз на 30с — вікно перевірки (Minute == 0) ширше за інтервал опитування,
                // тож 2:00 гарантовано буде спіймано. Після спрацювання чекаємо цілу хвилину,
                // інакше наступна ітерація циклу (за 30с) знову потрапить у ту саму хвилину й запустить перевірки вдруге.
                if (now.Hour == 2 && now.Minute == 0)
                {
                    _logger.Info("Starting scheduled service checks");

                    await RunChecks(ct);

                    await Task.Delay(TimeSpan.FromMinutes(1), ct);
                }

                await Task.Delay(TimeSpan.FromSeconds(30), ct);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                _logger.Warn("Background job was cancelled.");
                break;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Background job processing failed.");
            }
        }
    }

    private async Task RunChecks(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ISentinelMonitoringDbContext>();

        var checkIds = await dbContext.Checks
            .Where(x => x.IsEnabled)
            .Select(x => x.Id)
            .ToListAsync(ct);

        await Parallel.ForEachAsync(checkIds,
            new ParallelOptions
            {
                MaxDegreeOfParallelism = 10,
                CancellationToken = ct
            },
            async (checkId, token) =>
            {
                using var checkScope = _scopeFactory.CreateScope();

                var executor = checkScope.ServiceProvider.GetRequiredService<ICheckExecutor>();

                try
                {
                    await executor.ExecuteScheduleAsync(checkId, token);

                    _logger.Info("Check {CheckId} completed successfully", checkId);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Check {CheckId} failed", checkId);
                }
            });
    }
}
