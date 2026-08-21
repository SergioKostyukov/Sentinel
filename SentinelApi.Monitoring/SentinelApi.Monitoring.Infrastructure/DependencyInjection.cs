using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SentinelApi.Monitoring.Application.Interfaces.Infrastructure;
using SentinelApi.Monitoring.Infrastructure.Background;
using SentinelApi.Monitoring.Infrastructure.EmailSender;

namespace SentinelApi.Monitoring.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Background services
        services.AddHostedService<CheckSchedulerBackgroundService>();
        services.AddHostedService<CheckReportBackgroundService>();

        // SMTP server
        services.Configure<EmailSenderConfig>(configuration.GetSection("Smtp"));
        services.AddScoped<IEmailSenderService, EmailSenderService>();

        return services;
    }
}
