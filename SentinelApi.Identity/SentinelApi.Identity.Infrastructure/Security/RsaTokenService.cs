using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SentinelApi.Identity.Application.Interfaces.Infrastructure;
using SentinelApi.Identity.Domain.Entities;

namespace SentinelApi.Identity.Infrastructure.Security;

public sealed class RsaTokenService(IJwtKeyProvider keyProvider, IOptions<JwtIssuingOptions> jwtOptions) : ITokenService
{
    private readonly JwtIssuingOptions _jwtOptions = jwtOptions.Value;

    public AccessToken IssueAccessToken(User user)
    {
        var expiresAtUtc = DateTime.UtcNow.Add(_jwtOptions.AccessTokenLifetime);

        var credentials = new SigningCredentials(new RsaSecurityKey(keyProvider.GetKey()), SecurityAlgorithms.RsaSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Email, user.Email),
        };

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: expiresAtUtc,
            signingCredentials: credentials);

        var value = new JwtSecurityTokenHandler().WriteToken(token);

        return new AccessToken(value, expiresAtUtc);
    }
}
