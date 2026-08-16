namespace SentinelApi.Identity.Models;

public sealed record LoginRequest(
    string Login,
    string Password
);
