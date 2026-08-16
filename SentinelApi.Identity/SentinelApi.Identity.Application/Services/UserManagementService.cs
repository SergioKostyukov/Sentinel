using SentinelApi.Identity.Application.Contracts;
using SentinelApi.Identity.Application.Interfaces;
using SentinelApi.Identity.Application.Interfaces.Infrastructure;
using SentinelApi.Identity.Application.Mappers;
using SentinelApi.Identity.Domain.Enums;
using SentinelApi.Identity.Domain.Exceptions;

namespace SentinelApi.Identity.Application.Services;

public sealed class UserManagementService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher) : IUserManagementService
{
    public async Task<IReadOnlyList<UserSummaryResponse>> GetUsersAsync(CancellationToken ct)
    {
        var users = await userRepository.GetAllWithActionLogsAsync(ct);

        return [.. users.Select(u => new UserSummaryResponse(
            Id: u.Id,
            Login: u.Login,
            Email: u.Email,
            CreatedAtUtc: u.CreatedAtUtc,
            LastLoginAtUtc: u.ActionLogsAsAuthor
                .Where(a => a.ActionType == ActionType.Login)
                .Select(a => (DateTime?)a.DateTime)
                .Max()
        ))];
    }

    public async Task CreateUserAsync(CreateUserCommand command, CancellationToken ct)
    {
        if (await userRepository.ExistsByLoginAsync(command.Login, ct))
            throw new DuplicateUserException(command.Login);

        var passwordHash = passwordHasher.Hash(command.Password);
        var user = command.ToEntity(passwordHash);

        await userRepository.AddAsync(user, ct);
    }
}
