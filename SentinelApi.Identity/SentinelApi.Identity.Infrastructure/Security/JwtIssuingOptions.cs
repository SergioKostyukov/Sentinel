namespace SentinelApi.Identity.Infrastructure.Security;

public sealed class JwtIssuingOptions
{
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string PrivateKey { get; init; }
    public required TimeSpan AccessTokenLifetime { get; init; }
}
