using SentinelApi.Identity.Application.Contracts;

namespace SentinelApi.Identity.Application.Interfaces;

public interface IAuthService
{
    /// <summary>
    /// Перевіряє облікові дані та повертає токен доступу.
    /// </summary>
    Task<AuthResponse> LoginAsync(LoginCommand command, CancellationToken ct);
}
