namespace SentinelLib.Identity.Security.CurrentUser;

public interface ICurrentUserAccessor
{
    Guid UserId { get; }
    string Login { get; }
    string Email { get; }
}
