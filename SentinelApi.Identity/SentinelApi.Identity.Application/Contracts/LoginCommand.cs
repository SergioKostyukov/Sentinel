namespace SentinelApi.Identity.Application.Contracts;

public sealed record LoginCommand(
    string Login,
    string Password
);
