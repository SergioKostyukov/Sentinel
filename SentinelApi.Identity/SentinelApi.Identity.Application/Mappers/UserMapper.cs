using SentinelApi.Identity.Application.Contracts;
using SentinelApi.Identity.Domain.Entities;

namespace SentinelApi.Identity.Application.Mappers;

internal static class UserMapper
{
    internal static User ToEntity(this CreateUserCommand command, string passwordHash)
        => new User(
            login: command.Login,
            email: command.Email,
            passwordHash: passwordHash
        );
}
