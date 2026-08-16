using SentinelApi.Identity.Application.Contracts;

namespace SentinelApi.Identity.Application.Interfaces;

public interface IUserManagementService
{
    /// <summary>
    /// Повертає список усіх користувачів разом з часом останнього входу.
    /// </summary>
    Task<IReadOnlyList<UserSummaryResponse>> GetUsersAsync(CancellationToken ct);

    /// <summary>
    /// Створює нового користувача.
    /// </summary>
    Task CreateUserAsync(CreateUserCommand command, CancellationToken ct);
}
