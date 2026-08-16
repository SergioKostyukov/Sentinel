namespace SentinelApi.Identity.Application.Contracts;

public sealed record CreateUserCommand(
    string Login,
    string Email,
    string Password
);
