using Microsoft.AspNetCore.Identity;
using SentinelApi.Identity.Application.Interfaces.Infrastructure;
using SentinelApi.Identity.Domain.Entities;

namespace SentinelApi.Identity.Infrastructure.Security;

public sealed class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<User> _hasher = new();

    public string Hash(string password)
        => _hasher.HashPassword(null!, password);

    public bool Verify(string passwordHash, string password)
        => _hasher.VerifyHashedPassword(null!, passwordHash, password) != PasswordVerificationResult.Failed;
}
