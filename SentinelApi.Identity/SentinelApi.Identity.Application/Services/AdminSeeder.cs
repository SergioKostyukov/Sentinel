using SentinelApi.Identity.Application.Interfaces;
using SentinelApi.Identity.Application.Interfaces.Infrastructure;
using SentinelApi.Identity.Domain.Entities;

namespace SentinelApi.Identity.Application.Services;

public sealed class AdminSeeder(IUserRepository userRepository, IPasswordHasher passwordHasher) : IAdminSeeder
{
    public async Task SeedAsync(string login, string email, string password, CancellationToken ct)
    {
        var existingUsers = await userRepository.GetAllAsync(ct);
        if (existingUsers.Count > 0)
            return;

        var admin = new User(login, email, passwordHasher.Hash(password));

        await userRepository.AddAsync(admin, ct);
    }
}
