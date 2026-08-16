using NSubstitute;
using SentinelApi.Identity.Application.Interfaces.Infrastructure;
using SentinelApi.Identity.Application.Services;
using SentinelApi.Identity.Domain.Entities;
using Xunit;

namespace SentinelApi.Identity.Application.Tests;

public sealed class AdminSeederTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();
    private readonly AdminSeeder _sut;

    public AdminSeederTests()
    {
        _sut = new AdminSeeder(_userRepository, _passwordHasher);
    }

    [Fact]
    public async Task SeedAsync_WhenNoUsersExist_CreatesAdmin()
    {
        _userRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns([]);
        _passwordHasher.Hash("password").Returns("hashed");

        await _sut.SeedAsync("admin", "admin@sentinel.local", "password", CancellationToken.None);

        await _userRepository.Received(1).AddAsync(
            Arg.Is<User>(u => u.Login == "admin" && u.Email == "admin@sentinel.local" && u.PasswordHash == "hashed"),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SeedAsync_WhenUsersAlreadyExist_DoesNothing()
    {
        var existingUser = new User("login", "a@b.com", "hashed");
        _userRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns([existingUser]);

        await _sut.SeedAsync("admin", "admin@sentinel.local", "password", CancellationToken.None);

        await _userRepository.DidNotReceive().AddAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }
}
