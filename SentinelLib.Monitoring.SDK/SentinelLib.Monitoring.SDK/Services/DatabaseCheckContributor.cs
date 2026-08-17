using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SentinelLib.Monitoring.SDK.Abstractions;
using SentinelLib.Monitoring.SDK.Contracts;
using SentinelLib.Monitoring.SDK.Enums;

namespace SentinelLib.Monitoring.SDK.Services;

/// <summary>
/// Дефолтна перевірка працездатності бази даних.
/// </summary>
/// 
/// <typeparam name="TContext">
/// Тип контексту бази даних, який успадковується від <see cref="DbContext"/>.
/// </typeparam>
/// 
/// <remarks>
/// Використовується для швидкого додавання стандартної перевірки підключення до бази даних у Sentinel Monitoring.
///
/// Перевірка виконує:
/// <list type="bullet">
/// <item>
/// <description>
/// Виклик <see cref="DatabaseFacade.CanConnectAsync(CancellationToken)"/> для визначення доступності бази даних.
/// </description>
/// </item>
/// </list>
/// 
/// </remarks>
public sealed class DatabaseCheckContributor<TContext>(TContext dbContext, string name) : IHealthCheckContributor where TContext : DbContext
{
    private readonly TContext _dbContext = dbContext;

    public async Task<ServiceComponentCheck> CheckAsync(CancellationToken ct)
    {
        try
        {
            var canConnect = await _dbContext.Database.CanConnectAsync(ct);

            return new ServiceComponentCheck(
                Name: name,
                Status: canConnect ? HealthStatus.Healthy : HealthStatus.Unhealthy,
                Details: [new CheckDetail("Connection", canConnect ? "OK" : "Failed")]);
        }
        catch (Exception ex)
        {
            return new ServiceComponentCheck(
                Name: name,
                Status: HealthStatus.Unhealthy,
                Details: [new CheckDetail("Error", ex.Message)]);
        }
    }
}
