# API Reference

[← Back to README](../README.md)

## Abstractions

| Interface | Description |
| --- | --- |
| `IServiceCheckContributor` | Спільний базовий контракт перевірки компонента сервісу |
| `IHealthCheckContributor` | Контракт для власної health-check перевірки (`GET /api/sentinel/check`) |
| `ISnapshotCheckContributor` | Контракт для власної snapshot-check перевірки (`GET /api/sentinel/snapshot`) |
| `IServiceCheckProvider` | Спільний базовий контракт агрегатора перевірок |
| `IHealthCheckProvider` | Агрегатор усіх зареєстрованих `IHealthCheckContributor` |
| `ISnapshotCheckProvider` | Агрегатор усіх зареєстрованих `ISnapshotCheckContributor` |

Health-check та snapshot-check мають однакову модель відповіді та обробки (`ServiceCheckResponse`),
але реєструються та агрегуються незалежно й доступні за різними ендпоінтами.
Детальніше про реалізацію власних перевірок — [Custom Checks](custom-checks.md).

## Contracts

| Contract | Description |
| --- | --- |
| `ServiceCheckResponse` | Загальний результат перевірки сервісу |
| `ServiceComponentCheck` | Результат перевірки окремого компонента сервісу |
| `CheckDetail` | Додаткова інформація про результат перевірки |

## Enums

| Enum | Description |
| --- | --- |
| `HealthStatus` | Поточний стан функціонування сервісу або компонента |
| `CheckDetailType` | Тип значення додаткової інформації |

## Extensions

| Extension | Description |
| --- | --- |
| `AddSentinelMonitoring(IConfiguration, sectionName)` | Реєстрація сервісів Sentinel з налаштуваннями із секції конфігурації |
| `AddSentinelMonitoring(Action<SentinelMonitoringOptions>)` | Реєстрація сервісів Sentinel з налаштуваннями через делегат |
| `AddSentinelDatabaseCheck<TContext>()` | Додавання стандартної перевірки БД як health-check |
| `MapSentinelMonitoring()` | Реєстрація обох HTTP endpoint'ів — `check` і `snapshot` (захищені API-ключем) |
| `MapSentinelHealthCheck()` | Реєстрація лише `GET /api/sentinel/check` |
| `MapSentinelSnapshotCheck()` | Реєстрація лише `GET /api/sentinel/snapshot` |
| `RequireSentinelApiKey()` | Захист власного endpoint'а (minimal API) тим самим API-ключем |

Детальніше про налаштування — [Configuration](configuration.md).

## Security

| Extension | Description |
| --- | --- |
| `SentinelApiKeyEndpointFilter` | Endpoint filter, що перевіряє API-ключ у заголовку запиту |
| `SentinelHeaders` | Назви HTTP-заголовків запитів |
| `SentinelMonitoringOptions` | Налаштування бібліотеки (`ApiKey`), біндяться через `IOptions<T>` |

Детальніше про авторизацію — [Authorization](authorization.md).

## Services

| Service | Description |
| --- | --- |
| `DatabaseCheckContributor<TContext>` | Стандартна перевірка доступності бази даних через Entity Framework Core (реалізує контракт `IHealthCheckContributor`) |
| `ServiceCheckProviderBase` | Спільна логіка агрегації результатів перевірок |
| `HealthCheckProvider` | Збір результатів зареєстрованих health-check перевірок |
| `SnapshotCheckProvider` | Збір результатів зареєстрованих snapshot-check перевірок |
