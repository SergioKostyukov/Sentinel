using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace SentinelLib.Identity.Security.CurrentUser;

/// <summary>
/// Доступ до даних поточного авторизованого користувача.
/// </summary>
public sealed class CurrentUserAccessor(IHttpContextAccessor httpContextAccessor) : ICurrentUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Guid UserId => Guid.Parse(GetClaim(ClaimTypes.NameIdentifier));
    public string Login => GetClaim(ClaimTypes.Name);
    public string Email => GetClaim(ClaimTypes.Email);

    private ClaimsPrincipal User
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user?.Identity?.IsAuthenticated != true)
                throw new UnauthorizedAccessException();

            return user;
        }
    }

    private string GetClaim(string claimType)
        => User.FindFirst(claimType)?.Value ?? throw new InvalidOperationException($"Claim '{claimType}' not found.");
}
