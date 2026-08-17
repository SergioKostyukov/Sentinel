using SentinelLib.Monitoring.SDK.Abstractions;
using SentinelLib.Monitoring.SDK.Contracts;
using SentinelLib.Monitoring.SDK.Enums;

namespace SentinelLib.Monitoring.SDK.Services;

/// <summary>
/// Спільна логіка агрегації результатів <see cref="IServiceCheckContributor"/> у <see cref="ServiceCheckResponse"/>.
/// </summary>
public abstract class ServiceCheckProviderBase(IEnumerable<IServiceCheckContributor> contributors) : IServiceCheckProvider
{
    private readonly IEnumerable<IServiceCheckContributor> _contributors = contributors;

    public async Task<ServiceCheckResponse> CheckAsync(CancellationToken ct)
    {
        var checks = _contributors.Select(c => RunSafelyAsync(c, ct));
        var components = await Task.WhenAll(checks);

        return new ServiceCheckResponse(DateTime.UtcNow, components);
    }

    private static async Task<ServiceComponentCheck> RunSafelyAsync(IServiceCheckContributor contributor, CancellationToken ct)
    {
        try
        {
            return await contributor.CheckAsync(ct);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return new ServiceComponentCheck(
                Name: contributor.GetType().Name,
                Status: HealthStatus.Unhealthy,
                Details: [],
                Description: "Check threw an unhandled exception.");
        }
    }
}

/// <summary>
/// Агрегує результати усіх зареєстрованих <see cref="IHealthCheckContributor"/> для health-check.
/// </summary>
public sealed class HealthCheckProvider(IEnumerable<IHealthCheckContributor> contributors) : ServiceCheckProviderBase(contributors), IHealthCheckProvider
{
}

/// <summary>
/// Агрегує результати усіх зареєстрованих <see cref="ISnapshotCheckContributor"/> для snapshot-check.
/// </summary>
public sealed class SnapshotCheckProvider(IEnumerable<ISnapshotCheckContributor> contributors) : ServiceCheckProviderBase(contributors), ISnapshotCheckProvider
{
}
