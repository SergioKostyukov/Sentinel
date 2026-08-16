using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SentinelLib.Identity.Security.CurrentUser;

namespace SentinelLib.Identity.Security.Authentication;

/// <summary>
/// Розширення для налаштування JWT Bearer Authentication.
/// </summary>
public static class JwtAuthenticationExtensions
{
    public static IServiceCollection AddSentinelAuthentication(this IServiceCollection services, IConfiguration configuration, string sectionName = "Jwt", bool requireHttpsMetadata = true)
    {
        var jwtOptions = configuration
            .GetSection(sectionName)
            .Get<JwtValidationOptions>()
            ?? throw new InvalidOperationException();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwt =>
                {
                    jwt.Authority = jwtOptions.Issuer;
                    jwt.RequireHttpsMetadata = requireHttpsMetadata;

                    jwt.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,

                            ValidAudience = jwtOptions.Audience,
                        };
                });

        services.AddAuthorizationBuilder();

        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();

        return services;
    }
}
