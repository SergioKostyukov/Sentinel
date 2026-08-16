using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SentinelApi.Identity.Application.Interfaces.Infrastructure;
using SentinelApi.Identity.Infrastructure.Security;

namespace SentinelApi.Identity.Controllers;

/// <summary>
/// Публічні метадані для перевірки виданих токенів (OpenID Connect discovery / JWKS).
/// </summary>
[ApiController]
[Route(".well-known")]
public sealed class WellKnownController(IJwtKeyProvider keyProvider, IOptions<JwtIssuingOptions> options) : ControllerBase
{
    /// <summary>
    /// Повертає базові OpenID Connect метадані сервісу.
    /// </summary>
    [HttpGet("openid-configuration")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetOpenIdConfiguration()
    {
        var issuer = options.Value.Issuer;

        return Ok(new
        {
            issuer,
            jwks_uri = $"{issuer.TrimEnd('/')}/.well-known/jwks.json",
        });
    }

    /// <summary>
    /// Повертає публічний ключ у форматі JWKS для перевірки підпису токенів.
    /// </summary>
    [HttpGet("jwks.json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetJwks()
    {
        // Експортуємо лише публічну частину ключа, щоб приватний ключ ніколи не покидав цей сервіс.
        var publicParameters = keyProvider.GetKey().ExportParameters(includePrivateParameters: false);
        using var publicRsa = System.Security.Cryptography.RSA.Create(publicParameters);

        var jwk = JsonWebKeyConverter.ConvertFromRSASecurityKey(new RsaSecurityKey(publicRsa));
        jwk.Use = "sig";
        jwk.Alg = SecurityAlgorithms.RsaSha256;

        return Ok(new { keys = new[] { jwk } });
    }
}
