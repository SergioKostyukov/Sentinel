namespace SentinelLib.Identity.Security.Authentication;

public sealed class JwtValidationOptions
{
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
}
