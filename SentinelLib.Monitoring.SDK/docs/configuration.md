# Configuration

[← Back to README](../README.md)

## Реєстрація базових сервісів

Метод `AddSentinelMonitoring` має два overload'и — оберіть залежно від того, звідки зручніше брати налаштування.

### Через секцію конфігурації

```csharp
builder.Services.AddSentinelMonitoring(builder.Configuration);
```

За замовчуванням читає секцію `"Sentinel"`. Назву секції можна змінити:

```csharp
builder.Services.AddSentinelMonitoring(builder.Configuration, sectionName: "MyCustomSection");
```

`appsettings.json`:

```json
{
  "Sentinel": {
    "ApiKey": "ваш-ключ"
  }
}
```

### Через делегат

```csharp
builder.Services.AddSentinelMonitoring(options =>
{
    options.ApiKey = builder.Configuration["Sentinel:ApiKey"]!;
});
```

Обидва overload'и реєструють `IHealthCheckProvider`, `ISnapshotCheckProvider` та налаштування
`SentinelMonitoringOptions` через `IOptions<T>`.

---

## Health-check і snapshot-check

Бібліотека підтримує два незалежні типи перевірок з однаковою моделлю відповіді (`ServiceCheckResponse`)
та обробки, але різними наборами контриб'юторів і різними ендпоінтами:

| Тип | Контриб'ютор | Провайдер | Endpoint |
| --- | --- | --- | --- |
| Health-check | `IHealthCheckContributor` | `IHealthCheckProvider` | `GET /api/sentinel/check` |
| Snapshot-check | `ISnapshotCheckContributor` | `ISnapshotCheckProvider` | `GET /api/sentinel/snapshot` |

Детальніше про реалізацію власних перевірок — [Custom Checks](custom-checks.md).

---

## Реєстрація HTTP endpoint'ів

```csharp
app.MapSentinelMonitoring();
```

Реєструє `GET /api/sentinel/check` та `GET /api/sentinel/snapshot`, які повертають `ServiceCheckResponse`
з результатами зареєстрованих health-check та snapshot-check перевірок відповідно.
Обидва endpoint'и захищені API-ключем — див. [Authorization](authorization.md).

Якщо сервісу потрібен лише один із типів (наприклад, є тільки health-check перевірки, а snapshot-check
для нього не має сенсу), мапте відповідний endpoint окремо — другий тоді не реєструється взагалі:

```csharp
app.MapSentinelHealthCheck();   // тільки GET /api/sentinel/check
// або
app.MapSentinelSnapshotCheck(); // тільки GET /api/sentinel/snapshot
```

`AddSentinelMonitoring` при цьому все одно реєструє в DI обидва провайдери (`IHealthCheckProvider` та
`ISnapshotCheckProvider`) — це нешкідливо навіть без жодного контриб'ютора відповідного типу (провайдер
просто поверне порожній список `Components`), а мапиться лише той endpoint, який ви явно підключили.

---

## Підключення перевірки бази даних

Якщо сервіс використовує Entity Framework Core, можна додати стандартну перевірку доступності БД —
як health-check:

```csharp
builder.Services.AddSentinelDatabaseCheck<ApplicationDbContext>("MainDatabase");
```

Параметр `name` — довільна назва, під якою перевірка відображатиметься у відповіді endpoint'а (`ServiceComponentCheck.Name`).

Реалізація — `DatabaseCheckContributor<TContext>`, зареєстрована як `IHealthCheckContributor`.
