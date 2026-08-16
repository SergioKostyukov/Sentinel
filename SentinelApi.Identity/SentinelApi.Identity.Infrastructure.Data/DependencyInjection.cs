using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SentinelApi.Identity.Application.Interfaces.Infrastructure;
using SentinelApi.Identity.Infrastructure.Data.DbContexts;
using SentinelApi.Identity.Infrastructure.Data.Repositories;

namespace SentinelApi.Identity.Infrastructure.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityInfrastructureData(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultConnection = configuration.GetConnectionString("Identity")
            ?? throw new InvalidOperationException("Failed to load Identity connection string");

        services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(defaultConnection));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IActionLogRepository, ActionLogRepository>();

        return services;
    }
}
