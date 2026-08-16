using SentinelApi.Identity.Domain.Entities;

namespace SentinelApi.Identity.Application.Interfaces.Infrastructure;

public interface ITokenService
{
    /// <summary>
    /// Видає підписаний токен доступу для вказаного користувача.
    /// </summary>
    AccessToken IssueAccessToken(User user);
}

public sealed record AccessToken(string Value, DateTime ExpiresAtUtc);
