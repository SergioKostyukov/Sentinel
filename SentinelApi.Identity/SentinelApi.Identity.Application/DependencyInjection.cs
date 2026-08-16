using Microsoft.Extensions.DependencyInjection;
using SentinelApi.Identity.Application.Interfaces;
using SentinelApi.Identity.Application.Services;

namespace SentinelApi.Identity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserManagementService, UserManagementService>();
        services.AddScoped<IAdminSeeder, AdminSeeder>();

        return services;
    }
}
