# Custom Checks

[← Back to README](../README.md)

Кожен сервіс може мати декілька компонентів (Database, Background Jobs, External Api, Cache тощо).

## Health-check і snapshot-check

Бібліотека підтримує два типи перевірок з однаковою моделлю відповіді та обробки:

- **health-check** — реалізується через `IHealthCheckContributor`, агрегується `IHealthCheckProvider`,
  доступний за `GET /api/sentinel/check`;
- **snapshot-check** — реалізується через `ISnapshotCheckContributor`, агрегується `ISnapshotCheckProvider`,
  доступний за `GET /api/sentinel/snapshot`.

Обидва інтерфейси успадковують спільний контракт `IServiceCheckContributor` і мають однакову сигнатуру:

```csharp
public interface IServiceCheckContributor
{
    Task<ServiceComponentCheck> CheckAsync(CancellationToken ct);
}
```

Реалізовувати `IServiceCheckContributor` напряму не потрібно — оберіть `IHealthCheckContributor` чи
`ISnapshotCheckContributor` залежно від того, до якого ендпоінта має потрапити перевірка. Якщо перевірка
доречна для обох ендпоінтів, реалізація може успадковувати обидва інтерфейси одночасно (так зроблено у
`DatabaseCheckContributor<TContext>`).

## Приклад: перевірка зовнішнього API (health-check)

```csharp
public sealed class ExternalApiCheckContributor(IHttpClientFactory httpClientFactory) : IHealthCheckContributor
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<ServiceComponentCheck> CheckAsync(CancellationToken ct)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://external-api.example.com/health", ct);

            return new ServiceComponentCheck(
                Name: "ExternalApi",
                Status: response.IsSuccessStatusCode ? HealthStatus.Healthy : HealthStatus.Unhealthy,
                Details:
                [
                    new CheckDetail("StatusCode", ((int)response.StatusCode).ToString())
                ]
            );
        }
        catch (Exception ex)
        {
            return new ServiceComponentCheck(
                Name: "ExternalApi",
                Status: HealthStatus.Unhealthy,
                Details: [],
                Description: ex.Message
            );
        }
    }
}
```

## Реєстрація

```csharp
builder.Services.AddScoped<IHealthCheckContributor, ExternalApiCheckContributor>();
```

Для snapshot-check реєстрація аналогічна, лише через `ISnapshotCheckContributor`:

```csharp
builder.Services.AddScoped<ISnapshotCheckContributor, ExternalApiSnapshotContributor>();
```

Усі зареєстровані `IHealthCheckContributor` автоматично агрегуються через `IHealthCheckProvider` і
потрапляють у відповідь `GET /api/sentinel/check`. Аналогічно `ISnapshotCheckContributor` — через
`ISnapshotCheckProvider` у відповідь `GET /api/sentinel/snapshot`.

Для додавання ще одного, третього типу перевірки (нового ендпоінта) потрібно за тим самим зразком
реалізувати окремий маркерний інтерфейс контриб'ютора (аналог `IHealthCheckContributor`) та провайдер,
що буде їх агрегувати (аналог `IHealthCheckProvider`/`HealthCheckProvider`).
До ендпоінта також треба додати авторизацію через API-ключ, детальніше див. [Authorization](authorization.md).

## Стандартна перевірка БД

Для Entity Framework Core доступна готова реалізація — див. [Configuration](configuration.md).
