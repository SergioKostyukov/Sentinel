using SentinelApi.Monitoring.Domain.Entities;

namespace SentinelApi.Monitoring.Application.Interfaces;

public interface ICheckExecutor
{
    /// <summary>
    /// Виконує перевірку за запитом користувача (кнопка "Start" в UI).
    /// </summary>
    Task<CheckResult> ExecuteManualAsync(int checkId, CancellationToken ct);

    /// <summary>
    /// Виконує перевірку за розкладом, викликається фоновим сервісом.
    /// </summary>
    Task<CheckResult> ExecuteScheduleAsync(int checkId, CancellationToken ct);
}
