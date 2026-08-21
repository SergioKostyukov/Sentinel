using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SentinelApi.Monitoring.Application.Interfaces;
using SentinelApi.Monitoring.Application.Services;
using SentinelLib.Monitoring.SDK.Security;

namespace SentinelApi.Monitoring.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SentinelMonitoringOptions>(configuration.GetSection("SentinelMonitoring"));

        services.AddScoped<IServiceDefinitionService, ServiceDefinitionService>();
        services.AddScoped<ICheckService, CheckService>();
        services.AddScoped<IHistoryService, HistoryService>();
        services.AddScoped<ICheckResultService, CheckResultService>();
        services.AddScoped<ICheckExecutor, CheckExecutor>();

        return services;
    }
}
