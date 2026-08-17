using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SentinelLib.Monitoring.SDK.Abstractions;
using SentinelLib.Monitoring.SDK.Contracts;
using SentinelLib.Monitoring.SDK.Security;
using SentinelLib.Monitoring.SDK.Services;

namespace SentinelLib.Monitoring.SDK.Extensions;

/// <summary>
/// Extension methods для інтеграції ASP.NET Core сервісів із Sentinel Monitoring.
/// </summary>
public static class SentinelMonitoringExtensions
{
    /// <summary>
    /// Реєструє <see cref="IServiceCheckProvider"/> та налаштування <see cref="SentinelMonitoringOptions"/>
    /// із секції конфігурації <paramref name="sectionName"/>.
    /// </summary>
    /// <param name="sectionName">Назва секції конфігурації в appsettings.json (за замовчуванням "Sentinel").</param>
    public static IServiceCollection AddSentinelMonitoring(this IServiceCollection services, IConfiguration configuration, string sectionName = "Sentinel")
    {
        services.AddScoped<IHealthCheckProvider, HealthCheckProvider>();
        services.AddScoped<ISnapshotCheckProvider, SnapshotCheckProvider>();

        services.Configure<SentinelMonitoringOptions>(configuration.GetSection(sectionName));

        return services;
    }

    /// <summary>
    /// Реєструє <see cref="IServiceCheckProvider"/> та налаштування <see cref="SentinelMonitoringOptions"/>
    /// через делегат <paramref name="configure"/>.
    /// <code>
    /// builder.Services.AddSentinelMonitoring(options => options.ApiKey = builder.Configuration["Sentinel:ApiKey"]!);
    /// </code>
    /// </summary>
    public static IServiceCollection AddSentinelMonitoring(this IServiceCollection services, Action<SentinelMonitoringOptions>? configure = null)
    {
        services.AddScoped<IHealthCheckProvider, HealthCheckProvider>();
        services.AddScoped<ISnapshotCheckProvider, SnapshotCheckProvider>();

        if (configure is not null)
        {
            services.Configure(configure);
        }

        return services;
    }
    /// <summary>
    /// Реєструє перевірку доступності бази даних <typeparamref name="TContext"/> через Entity Framework Core
    /// як health-check (потрапляє у відповідь <c>GET /api/sentinel/check</c>).
    /// <code>
    /// builder.Services.AddSentinelDatabaseCheck&lt;ApplicationDbContext&gt;("MainDatabase");
    /// </code>
    /// </summary>
    /// <param name="name">Назва перевірки, що відображається у результатах.</param>
    public static IServiceCollection AddSentinelDatabaseCheck<TContext>(this IServiceCollection services, string name = "Database") where TContext : DbContext
    {
        services.AddScoped<IHealthCheckContributor>(sp =>
            new DatabaseCheckContributor<TContext>(sp.GetRequiredService<TContext>(), name)
        );

        return services;
    }

    /// <summary>
    /// Реєструє <c>GET /api/sentinel/check</c> та <c>GET /api/sentinel/snapshot</c> — endpoint'и, що повертають
    /// <see cref="ServiceCheckResponse"/> із результатами зареєстрованих health-check та snapshot-check
    /// перевірок відповідно. Доступ до обох захищено API-ключем.
    /// </summary>
    /// <remarks>
    /// Якщо сервісу потрібен лише один із типів перевірки, замість цього методу викличте
    /// <see cref="MapSentinelHealthCheck"/> або <see cref="MapSentinelSnapshotCheck"/> окремо —
    /// тоді буде зареєстровано лише відповідний endpoint.
    /// </remarks>
    public static IEndpointRouteBuilder MapSentinelMonitoring(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapSentinelHealthCheck();
        endpoints.MapSentinelSnapshotCheck();

        return endpoints;
    }

    /// <summary>
    /// Реєструє <c>GET /api/sentinel/check</c> — endpoint, що повертає <see cref="ServiceCheckResponse"/>
    /// із результатами зареєстрованих health-check перевірок. Доступ захищено API-ключем.
    /// Використовуйте окремо від <see cref="MapSentinelMonitoring"/>, якщо сервісу не потрібен snapshot-check.
    /// </summary>
    public static IEndpointRouteBuilder MapSentinelHealthCheck(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            pattern: "/api/sentinel/check",
            handler: async (IHealthCheckProvider provider, CancellationToken ct) =>
                await provider.CheckAsync(ct))
            .RequireSentinelApiKey();

        return endpoints;
    }

    /// <summary>
    /// Реєструє <c>GET /api/sentinel/snapshot</c> — endpoint, що повертає <see cref="ServiceCheckResponse"/>
    /// із результатами зареєстрованих snapshot-check перевірок. Доступ захищено API-ключем.
    /// Використовуйте окремо від <see cref="MapSentinelMonitoring"/>, якщо сервісу не потрібен health-check.
    /// </summary>
    public static IEndpointRouteBuilder MapSentinelSnapshotCheck(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
            pattern: "/api/sentinel/snapshot",
            handler: async (ISnapshotCheckProvider provider, CancellationToken ct) =>
                await provider.CheckAsync(ct))
            .RequireSentinelApiKey();

        return endpoints;
    }

    /// <summary>
    /// Захищає одиничний minimal API endpoint перевіркою Sentinel API-ключа
    /// (заголовок <see cref="SentinelHeaders.ApiKey"/>).
    /// </summary>
    public static RouteHandlerBuilder RequireSentinelApiKey(this RouteHandlerBuilder builder)
    {
        builder.AddEndpointFilter<SentinelApiKeyEndpointFilter>();

        return builder;
    }

    /// <summary>
    /// Захищає групу minimal API endpoint'ів перевіркою Sentinel API-ключа
    /// (заголовок <see cref="SentinelHeaders.ApiKey"/>).
    /// </summary>
    public static RouteGroupBuilder RequireSentinelApiKey(this RouteGroupBuilder builder)
    {
        builder.AddEndpointFilter<SentinelApiKeyEndpointFilter>();

        return builder;
    }
}
