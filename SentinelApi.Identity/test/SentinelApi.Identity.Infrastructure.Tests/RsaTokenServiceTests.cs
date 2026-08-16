using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SentinelApi.Identity.Domain.Entities;
using SentinelApi.Identity.Infrastructure.Security;
using Xunit;

namespace SentinelApi.Identity.Infrastructure.Tests;

public sealed class RsaTokenServiceTests
{
    private static (RsaTokenService Sut, RSA Rsa, JwtIssuingOptions Options) CreateSut()
    {
        var rsa = RSA.Create(2048);
        var options = new JwtIssuingOptions
        {
            Issuer = "http://identity:8080",
            Audience = "SentinelApi",
            PrivateKey = rsa.ExportPkcs8PrivateKeyPem(),
            AccessTokenLifetime = TimeSpan.FromHours(12),
        };

        var keyProvider = new JwtKeyProvider(Options.Create(options));
        var sut = new RsaTokenService(keyProvider, Options.Create(options));

        return (sut, rsa, options);
    }

    [Fact]
    public void IssueAccessToken_ProducesTokenWithExpectedClaimsAndValidSignature()
    {
        var (sut, rsa, options) = CreateSut();
        var user = new User("login", "a@b.com", "hash");

        var token = sut.IssueAccessToken(user);

        var handler = new JwtSecurityTokenHandler();
        var principal = handler.ValidateToken(token.Value, new TokenValidationParameters
        {
            ValidIssuer = options.Issuer,
            ValidAudience = options.Audience,
            IssuerSigningKey = new RsaSecurityKey(rsa),
        }, out _);

        Assert.Equal(user.Id.ToString(), principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        Assert.Equal(user.Login, principal.FindFirst(ClaimTypes.Name)?.Value);
        Assert.Equal(user.Email, principal.FindFirst(ClaimTypes.Email)?.Value);
    }
}
