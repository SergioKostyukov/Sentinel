using NSubstitute;
using SentinelApi.Identity.Application.Contracts;
using SentinelApi.Identity.Application.Services;
using SentinelApi.Identity.Domain.Entities;
using SentinelApi.Identity.Domain.Enums;
using SentinelApi.Identity.Domain.Exceptions;
using Xunit;
using SentinelApi.Identity.Application.Interfaces.Infrastructure;

namespace SentinelApi.Identity.Application.Tests;

public sealed class AuthServiceTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IActionLogRepository _actionLogRepository = Substitute.For<IActionLogRepository>();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();
    private readonly ITokenService _tokenService = Substitute.For<ITokenService>();
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        _sut = new AuthService(_userRepository, _actionLogRepository, _passwordHasher, _tokenService);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsAccessToken()
    {
        var user = new User("login", "a@b.com", "hashed");
        _userRepository.FindByLoginAsync("login", Arg.Any<CancellationToken>()).Returns(user);
        _passwordHasher.Verify("hashed", "password").Returns(true);
        var expiresAt = DateTime.UtcNow.AddHours(1);
        _tokenService.IssueAccessToken(user).Returns(new AccessToken("jwt-token", expiresAt));

        var result = await _sut.LoginAsync(new LoginCommand("login", "password"), CancellationToken.None);

        Assert.Equal("jwt-token", result.AccessToken);
        Assert.Equal(expiresAt, result.ExpiresAtUtc);

        await _actionLogRepository.Received(1).AddAsync(
            Arg.Is<ActionLog>(a => a.AuthorId == user.Id && a.TargetId == user.Id && a.ActionType == ActionType.Login),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task LoginAsync_WhenUserNotFound_ThrowsInvalidCredentialsException()
    {
        _userRepository.FindByLoginAsync("login", Arg.Any<CancellationToken>()).Returns((User?)null);

        await Assert.ThrowsAsync<InvalidCredentialsException>(() =>
            _sut.LoginAsync(new LoginCommand("login", "password"), CancellationToken.None));
    }

    [Fact]
    public async Task LoginAsync_WhenPasswordIncorrect_ThrowsInvalidCredentialsException()
    {
        var user = new User("login", "a@b.com", "hashed");
        _userRepository.FindByLoginAsync("login", Arg.Any<CancellationToken>()).Returns(user);
        _passwordHasher.Verify("hashed", "wrong").Returns(false);

        await Assert.ThrowsAsync<InvalidCredentialsException>(() =>
            _sut.LoginAsync(new LoginCommand("login", "wrong"), CancellationToken.None));
    }
}
