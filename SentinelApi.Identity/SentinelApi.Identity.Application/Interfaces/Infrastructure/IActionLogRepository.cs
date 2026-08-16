using SentinelApi.Identity.Domain.Entities;

namespace SentinelApi.Identity.Application.Interfaces.Infrastructure;

public interface IActionLogRepository
{
    /// <summary>
    /// Додає новий запис журналу дій користувача.
    /// </summary>
    Task AddAsync(ActionLog actionLog, CancellationToken ct);
}
