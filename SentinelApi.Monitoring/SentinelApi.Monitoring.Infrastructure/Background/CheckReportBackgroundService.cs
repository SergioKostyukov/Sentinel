using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using SentinelApi.Monitoring.Application.Interfaces;

namespace SentinelApi.Monitoring.Infrastructure.Background;

/// <summary>
/// Фоновий сервіс для надсилання щоденного звіту про результати перевірок.
/// 1. Запускається щодня о 8:00 ранку.
/// 2. Формує звіт по результатам перевірок за попередній день та надсилає його на електронну пошту.
/// </summary>
public class CheckReportBackgroundService(IServiceScopeFactory scopeFactory) : BackgroundService
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
                // тож 8:00 гарантовано буде спіймано. Після спрацювання чекаємо цілу хвилину,
                // інакше наступна ітерація циклу (за 30с) знову потрапить у ту саму хвилину й надішле звіт вдруге.
                if (now.Hour == 8 && now.Minute == 0)
                {
                    _logger.Info("Sending daily report");

                    await SendReport(ct);

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

    private async Task SendReport(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();

        var service = scope.ServiceProvider.GetRequiredService<ICheckResultService>();

        await service.SendDailyReportAsync(DateTime.Now, ct);
    }
}
