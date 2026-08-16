using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SentinelApi.Identity.Application.Interfaces.Infrastructure;
using SentinelApi.Identity.Infrastructure.Security;

namespace SentinelApi.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtIssuingOptions>(configuration.GetSection("Jwt"));

        services.AddSingleton<IJwtKeyProvider, JwtKeyProvider>();
        services.AddSingleton<ITokenService, RsaTokenService>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        return services;
    }
}
