namespace SentinelApi.Identity.Application.Contracts;

public sealed record UserSummaryResponse(
    Guid Id,
    string Login,
    string Email,
    DateTime CreatedAtUtc,
    DateTime? LastLoginAtUtc
);
