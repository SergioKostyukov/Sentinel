namespace SentinelApi.Identity.Models;

public sealed record CreateUserRequest(
    string Login,
    string Email,
    string Password
);
