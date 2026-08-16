namespace SentinelApi.Identity.Application.Contracts;

public sealed record AuthResponse(
    string AccessToken,
    DateTime ExpiresAtUtc
);
