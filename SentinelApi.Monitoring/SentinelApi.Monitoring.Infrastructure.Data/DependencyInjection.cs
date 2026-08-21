using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SentinelApi.Monitoring.Application.Interfaces.Infrastructure;
using SentinelApi.Monitoring.Infrastructure.Data.DbContexts;
using SentinelApi.Monitoring.Infrastructure.Data.Interceptors;

namespace SentinelApi.Monitoring.Infrastructure.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureData(this IServiceCollection services, IConfiguration configuration)
    {
        var defaultConnection = configuration.GetConnectionString("Monitoring")
            ?? throw new Exception("Failed to load SentinelApi.Monitoring connection string");

        services.AddScoped<ChangeLoggingInterceptor>();

        services.AddDbContext<ISentinelMonitoringDbContext, SentinelMonitoringDbContext>((sp, options) =>
        {
            options.UseSqlServer(defaultConnection, options => options.MigrationsAssembly(typeof(SentinelMonitoringDbContext).Assembly.FullName)
                                                                      .CommandTimeout(300))
                   .AddInterceptors(sp.GetRequiredService<ChangeLoggingInterceptor>());
        });

        return services;
    }
}
