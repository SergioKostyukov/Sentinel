using SentinelApi.Identity.Domain.Entities;

namespace SentinelApi.Identity.Application.Interfaces.Infrastructure;

public interface IUserRepository
{
    /// <summary>
    /// Шукає користувача за логіном.
    /// </summary>
    Task<User?> FindByLoginAsync(string login, CancellationToken ct);

    /// <summary>
    /// Перевіряє, чи існує користувач із таким логіном.
    /// </summary>
    Task<bool> ExistsByLoginAsync(string login, CancellationToken ct);

    /// <summary>
    /// Повертає всіх користувачів без пов'язаних даних.
    /// </summary>
    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken ct);

    /// <summary>
    /// Повертає всіх користувачів разом з їхніми записами журналу дій.
    /// </summary>
    Task<IReadOnlyList<User>> GetAllWithActionLogsAsync(CancellationToken ct);

    /// <summary>
    /// Додає нового користувача.
    /// </summary>
    Task AddAsync(User user, CancellationToken ct);
}
