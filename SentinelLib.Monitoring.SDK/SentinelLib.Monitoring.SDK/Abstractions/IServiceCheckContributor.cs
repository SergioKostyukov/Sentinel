using SentinelLib.Monitoring.SDK.Contracts;

namespace SentinelLib.Monitoring.SDK.Abstractions;

/// <summary>
/// Перевірка стану окремого компонента сервісу (БД, зовнішній API, черга тощо).
/// Спільний базовий контракт для <see cref="IHealthCheckContributor"/> (health-check) та
/// <see cref="ISnapshotCheckContributor"/> (snapshot-check) — реалізовувати напряму не потрібно,
/// оберіть один із двох конкретних типів залежно від того, до якого ендпоінта має потрапити перевірка.
/// </summary>
public interface IServiceCheckContributor
{
    /// <summary>
    /// Виконує перевірку компонента.
    /// </summary>
    /// <returns>Результат перевірки конкретного компонента.</returns>
    Task<ServiceComponentCheck> CheckAsync(CancellationToken ct);
}

/// <summary>
/// Перевірка працездатності окремого компонента сервісу для health-check.
/// Реалізації реєструються через DI та агрегуються <see cref="IHealthCheckProvider"/>,
/// результат доступний за <c>GET /api/sentinel/check</c>.
/// </summary>
public interface IHealthCheckContributor : IServiceCheckContributor
{
}

/// <summary>
/// Перевірка стану окремого компонента сервісу для snapshot-check.
/// Реалізації реєструються через DI та агрегуються <see cref="ISnapshotCheckProvider"/>,
/// результат доступний за <c>GET /api/sentinel/snapshot</c>.
/// </summary>
public interface ISnapshotCheckContributor : IServiceCheckContributor
{
}
