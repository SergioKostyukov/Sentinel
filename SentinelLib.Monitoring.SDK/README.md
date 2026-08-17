# SentinelLib.Monitoring.SDK

SDK-бібліотека для інтеграції сервісів із системою **SentinelApi.Monitoring**: контракти відповіді, агрегація перевірок, готовий HTTP endpoint та авторизація через API-ключ.

## 🧩 Assignment

Бібліотека відповідає за:
- єдиний контракт відповіді для всіх сервісів;
- автоматичну агрегацію результатів перевірок;
- розширення через власні `IServiceCheckContributor`;
- стандартні перевірки критичних залежностей;
- готовий HTTP endpoint для Sentinel;
- авторизацію запитів до цього endpoint'а через API-ключ.

Бібліотека **не відповідає за**:
- виконання періодичних перевірок Sentinel;
- збереження результатів моніторингу;
- формування звітів;
- відправку повідомлень;
- планування background jobs.

Ця логіка знаходиться на стороні **SentinelApi.Monitoring**.

---

## ⚙️ Quick Start

### NuGet Package

```xml
<PackageReference Include="SentinelLib.Monitoring.SDK" Version="1.0.0" />
```

> Якщо сервіс має декілька проектів, то бібліотеку необхідно підключати до проекту, який реєструватиме кастомні перевірки і має доступ до DbContext (якщо він є і потребує перевірки).

### Program.cs сервісу
 
```csharp
builder.Services.AddSentinelMonitoring(builder.Configuration);
 
// ...
 
app.MapSentinelMonitoring();
```

> Розширені конфігурації бібліотеки — див. [Configuration](docs/configuration.md).

---

## 📚 Documentation
 
| Topic | Description |
| --- | --- |
| [Project Repository](https://github.com/SergioKostyukov/Sentinel) | Full project documentation |
| [Configuration](https://github.com/SergioKostyukov/Sentinel/tree/main/SentinelLib.Monitoring.SDK/docs/configuration.md) | Реєстрація сервісів, опції, підключення перевірки БД |
| [Authorization](https://github.com/SergioKostyukov/Sentinel/tree/main/SentinelLib.Monitoring.SDK/docs/authorization.md) | API-ключ: налаштування, генерація, захист власних endpoint'ів |
| [Custom Checks](https://github.com/SergioKostyukov/Sentinel/tree/main/SentinelLib.Monitoring.SDK/docs/custom-checks.md) | Як реалізувати власний `IServiceCheckContributor` |
| [API Reference](https://github.com/SergioKostyukov/Sentinel/tree/main/SentinelLib.Monitoring.SDK/docs/api-reference.md) | Повний перелік контрактів, інтерфейсів, extension-методів |
| [Examples](https://github.com/SergioKostyukov/Sentinel/tree/main/SentinelLib.Monitoring.SDK/docs/examples.md) | Приклади HTTP-запиту та відповіді endpoint'а |
| [Publishing](https://github.com/SergioKostyukov/Sentinel/tree/main/SentinelLib.Monitoring.SDK/docs/publishing.md) | Як зібрати та опублікувати нову версію NuGet-пакета |
