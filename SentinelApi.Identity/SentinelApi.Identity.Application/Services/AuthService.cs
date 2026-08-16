using SentinelApi.Identity.Application.Contracts;
using SentinelApi.Identity.Application.Interfaces;
using SentinelApi.Identity.Application.Interfaces.Infrastructure;
using SentinelApi.Identity.Domain.Entities;
using SentinelApi.Identity.Domain.Enums;
using SentinelApi.Identity.Domain.Exceptions;

namespace SentinelApi.Identity.Application.Services;

public sealed class AuthService(
    IUserRepository userRepository,
    IActionLogRepository actionLogRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService) : IAuthService
{
    public async Task<AuthResponse> LoginAsync(LoginCommand command, CancellationToken ct)
    {
        var user = await userRepository.FindByLoginAsync(command.Login, ct)
            ?? throw new InvalidCredentialsException();

        if (!passwordHasher.Verify(user.PasswordHash, command.Password))
            throw new InvalidCredentialsException();

        await actionLogRepository.AddAsync(new ActionLog(user.Id, user.Id, ActionType.Login, "User logged in."), ct);

        var token = tokenService.IssueAccessToken(user);

        return new AuthResponse(
            AccessToken: token.Value,
            ExpiresAtUtc: token.ExpiresAtUtc
        );
    }
}
