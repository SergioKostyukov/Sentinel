using NSubstitute;
using SentinelApi.Identity.Application.Contracts;
using SentinelApi.Identity.Application.Interfaces.Infrastructure;
using SentinelApi.Identity.Application.Services;
using SentinelApi.Identity.Domain.Entities;
using SentinelApi.Identity.Domain.Enums;
using SentinelApi.Identity.Domain.Exceptions;
using Xunit;

namespace SentinelApi.Identity.Application.Tests;

public sealed class UserManagementServiceTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();
    private readonly UserManagementService _sut;

    public UserManagementServiceTests()
    {
        _sut = new UserManagementService(_userRepository, _passwordHasher);
    }

    [Fact]
    public async Task CreateUserAsync_WhenLoginIsFree_AddsUser()
    {
        _userRepository.ExistsByLoginAsync("login", Arg.Any<CancellationToken>()).Returns(false);
        _passwordHasher.Hash("password").Returns("hashed");

        await _sut.CreateUserAsync(new CreateUserCommand("login", "a@b.com", "password"), CancellationToken.None);

        await _userRepository.Received(1).AddAsync(
            Arg.Is<User>(u => u.Login == "login" && u.Email == "a@b.com" && u.PasswordHash == "hashed"),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateUserAsync_WhenLoginTaken_ThrowsDuplicateUserException()
    {
        _userRepository.ExistsByLoginAsync("login", Arg.Any<CancellationToken>()).Returns(true);

        await Assert.ThrowsAsync<DuplicateUserException>(() =>
            _sut.CreateUserAsync(new CreateUserCommand("login", "a@b.com", "password"), CancellationToken.None));
    }

    [Fact]
    public async Task GetUsersAsync_MapsLastLoginFromActionLogs()
    {
        var user1 = new User("login1", "a@b.com", "hashed");
        var user2 = new User("login2", "c@d.com", "hashed");
        var earlierLogin = DateTime.UtcNow.AddDays(-1);
        var lastLogin = DateTime.UtcNow;

        user1.ActionLogsAsAuthor.Add(new ActionLog(user1.Id, user1.Id, ActionType.Login, "User logged in.") { DateTime = earlierLogin });
        user1.ActionLogsAsAuthor.Add(new ActionLog(user1.Id, user1.Id, ActionType.Login, "User logged in.") { DateTime = lastLogin });

        _userRepository.GetAllWithActionLogsAsync(Arg.Any<CancellationToken>()).Returns([user1, user2]);

        var result = await _sut.GetUsersAsync(CancellationToken.None);

        Assert.Equal(lastLogin, result.Single(u => u.Id == user1.Id).LastLoginAtUtc);
        Assert.Null(result.Single(u => u.Id == user2.Id).LastLoginAtUtc);
    }
}
