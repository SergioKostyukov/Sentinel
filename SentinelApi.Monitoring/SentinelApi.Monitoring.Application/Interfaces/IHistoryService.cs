using SentinelApi.Monitoring.Application.Models.History;

namespace SentinelApi.Monitoring.Application.Interfaces;

public interface IHistoryService
{
    /// <summary>
    /// Отримання інформації про історію запитів користувачів
    /// </summary>
    Task<GetActionLogListResponse> GetListAsync(GetActionLogListRequest request, CancellationToken ct);

    /// <summary>
    /// Записує в історію дію створення сервісу.
    /// </summary>
    Task SaveServiceDefinitionCreateActionLogAsync(string userId, string userLogin, string targetId, string targetName, string description, CancellationToken ct);

    /// <summary>
    /// Записує в історію дію створення перевірки.
    /// </summary>
    Task SaveCheckCreateActionLogAsync(string userId, string userLogin, string targetId, string targetName, string description, CancellationToken ct);

    /// <summary>
    /// Записує в історію дію ручного запуску перевірки.
    /// </summary>
    Task SaveServiceCheckTriggerActionLogAsync(string userId, string userLogin, string targetId, string targetName, string description, CancellationToken ct);
}
