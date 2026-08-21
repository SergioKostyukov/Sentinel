# SentinelApi.Monitoring

Back-end сервіс для SentinelUI (конфігурація та запуск перевірок).

## 🧩 Assignment

| Controller | Description	|
| --- | --- |
| **CheckController** | Адміністрування перевірок сервісів |
| **CheckResultController** | Отримання історії результатів перевірок |
| **HistoryController** | Отримання історії запитів користувачів сервісу |
| **OptionController** | Отримання опцій для полів вибору |
| **ServiceDefinitionController** | Адміністрування даних про сервіси, які моніторяться |

| BgService | Description |
| --- | --- |
| **CheckSchedulerBackgroundService** | Щоденна перевірка статусу сервісів |
| **CheckReportBackgroundService** | Щоденна відправка звітів оцінки статусу сервісів |

---

## 🔐 Authentication and Authorization

- **Type:** JWT на основі Bearer Token
- **Authentication:** виконується через верифікацію публічного ключа, отриманого з SentinelApi.Identity

---

## 🔗 Integrations

| Integration | Description |
| --- | --- |
| **MSSQL Server** | Основна база даних сервісу (`SentinelMonitoring`) |
| **SMTP** | Відправка звітних листів (`CheckReportBackgroundService`); локально — `smtp4dev` |
| **SentinelLib.Identity.Security** | Верифікація JWT, виданого `SentinelApi.Identity` |
| **SentinelLib.Monitoring.SDK** | Клієнт для опитування `/api/sentinel/check` на боці моніторованих сервісів (`CheckExecutor`) |

---

## ⚙️ Конфігурація

Як і Identity, застосунок підтримує два режими запуску, кожен зі своїм джерелом чутливих даних —
див. [CONTRIBUTING.md](../docs/CONTRIBUTING.md).

**Docker-режим** — через `.env` у корені репо (див. `.env.example`):

| Ключ у `.env` | Призначення |
| --- | --- |
| `MSSQL_SA_PASSWORD` | Пароль до спільного SQL Server (та сама база, окрема `Database=SentinelMonitoring`) |
| `SENTINEL_MONITORING_API_KEY` | Ключ, яким Monitoring авторизує запити до `/api/sentinel/check` моніторованих сервісів |
| `SMTP_USERNAME` / `SMTP_PASSWORD` | Облікові дані SMTP для відправки звітів |

У `docker-compose.yml` SMTP за замовчуванням вказує на локальний `smtp4dev` (сервіс `smtp4dev`,
порт `25` всередині мережі) — листи нікуди реально не йдуть, лише перехоплюються й показуються
у веб-UI на `http://localhost:5000`. Для реальної відправки треба підключити справжній SMTP-провайдер
(Gmail, SendGrid тощо) через ті самі змінні.

**Debug-режим** (`dotnet run` / IDE) — скопіювати `appsettings.Template.json` → `appsettings.Development.json`
і заповнити реальними значеннями. SQL Server і `smtp4dev` при цьому все одно піднімаються з docker-compose
(`docker compose up -d sql-server smtp4dev`) — `Smtp:Port` вказує на проброшений на хост порт `2525`, а не
на внутрішній докер-порт `25`. Якщо тестуєш разом з Identity — той теж має бути запущений локально
(`appsettings.Development.json` в Identity), інакше `Jwt:Issuer` не збігатиметься з issuer'ом токена
з docker-контейнера Identity, і Monitoring відповість `401`.

---

## 📝 Additional Notes

- Для прямого доступу до ендпоінтів та їх опису впроваджено swagger (для debug запуску).
- Валідація вхідних моделей реалізована через FluentValidation.
- Реалізований глобальний обробник помилок на основі паттерну ProblemDetails.
- Таблиці БД створюються/оновлюються автоматично при старті застосунку (`Database.MigrateAsync()`).

### Related Documentation

| Document | Description |
| --- | --- |
| [Project README](../README.md) | README (архітектура і повна документація проекту) |
