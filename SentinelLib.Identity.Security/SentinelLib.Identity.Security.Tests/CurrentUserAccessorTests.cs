using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SentinelLib.Identity.Security.CurrentUser;
using Xunit;

namespace SentinelLib.Identity.Security.Tests;

public sealed class CurrentUserAccessorTests
{
    private static CurrentUserAccessor CreateSut(HttpContext? httpContext)
    {
        var accessor = new HttpContextAccessor { HttpContext = httpContext };
        return new CurrentUserAccessor(accessor);
    }

    private static HttpContext CreateAuthenticatedContext(Guid userId, string login, string email)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, login),
            new Claim(ClaimTypes.Email, email),
        };
        var identity = new ClaimsIdentity(claims, authenticationType: "Test");

        return new DefaultHttpContext { User = new ClaimsPrincipal(identity) };
    }

    [Fact]
    public void Properties_WhenUserAuthenticatedWithAllClaims_ReturnExpectedValues()
    {
        var userId = Guid.NewGuid();
        var sut = CreateSut(CreateAuthenticatedContext(userId, "login", "a@b.com"));

        Assert.Equal(userId, sut.UserId);
        Assert.Equal("login", sut.Login);
        Assert.Equal("a@b.com", sut.Email);
    }

    [Fact]
    public void UserId_WhenNotAuthenticated_ThrowsUnauthorizedAccessException()
    {
        var sut = CreateSut(new DefaultHttpContext());

        Assert.Throws<UnauthorizedAccessException>(() => sut.UserId);
    }

    [Fact]
    public void Login_WhenClaimMissing_ThrowsInvalidOperationException()
    {
        var identity = new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())], authenticationType: "Test");
        var sut = CreateSut(new DefaultHttpContext { User = new ClaimsPrincipal(identity) });

        Assert.Throws<InvalidOperationException>(() => sut.Login);
    }
}
