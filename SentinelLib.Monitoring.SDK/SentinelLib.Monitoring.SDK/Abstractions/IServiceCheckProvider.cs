using SentinelLib.Monitoring.SDK.Contracts;

namespace SentinelLib.Monitoring.SDK.Abstractions;

/// <summary>
/// Агрегує результати усіх зареєстрованих <see cref="IServiceCheckContributor"/>
/// в єдину відповідь про стан сервісу.
/// Спільний базовий контракт для <see cref="IHealthCheckProvider"/> та <see cref="ISnapshotCheckProvider"/>.
/// </summary>
public interface IServiceCheckProvider
{
    /// <summary>
    /// Виконує перевірку усіх зареєстрованих компонентів сервісу.
    /// </summary>
    /// <returns>Зведений результат перевірки усіх компонентів.</returns>
    Task<ServiceCheckResponse> CheckAsync(CancellationToken ct);
}

/// <summary>
/// Агрегує результати усіх зареєстрованих <see cref="IHealthCheckContributor"/>
/// в єдину відповідь health-check (<c>GET /api/sentinel/check</c>).
/// </summary>
public interface IHealthCheckProvider : IServiceCheckProvider
{
}

/// <summary>
/// Агрегує результати усіх зареєстрованих <see cref="ISnapshotCheckContributor"/>
/// в єдину відповідь snapshot-check (<c>GET /api/sentinel/snapshot</c>).
/// </summary>
public interface ISnapshotCheckProvider : IServiceCheckProvider
{
}
