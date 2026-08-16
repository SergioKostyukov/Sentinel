# SentinelApi.Identity

Сервіс автентифікації для системи Sentinel: логін користувачів, видача JWT (RSA-підписаних), керування юзерами, публікація OpenID discovery та JWKS для інших сервісів.

---

## 🧩 Assignment

Сервіс відповідає за:
- логін користувачів (`/api/auth/login`);
- видачу access-токенів (JWT, підписаних RSA-ключем);
- керування юзерами: список (`GET /api/users`), створення (`POST /api/users`);
- публікацію метаданих для валідації токенів іншими сервісами (`/.well-known/openid-configuration`, `/.well-known/jwks.json`).

Валідацію токенів на боці інших сервісів виконує бібліотека [`SentinelLib.Identity.Security`](../SentinelLib.Identity.Security/SentinelLib.Identity.Security/README.md).

### Related Documentation

| Document | Description |
| --- | --- |
| [Project README](../README.md) | Архітектура і повна документація проєкту |
| [SentinelLib.Identity.Security](../SentinelLib.Identity.Security/SentinelLib.Identity.Security/README.md) | Бібліотека валідації токенів для інших сервісів |

---

## ⚙️ Конфігурація

Застосунок підтримує два режими запуску, кожен зі своїм джерелом чутливих даних:

| Режим | Джерело чутливих даних | Шаблон |
| --- | --- | --- |
| Debug (`dotnet run` / IDE) | `appsettings.Development.json` (не комітиться) | `appsettings.Template.json` |
| Docker (`docker compose`) | `.env` (не комітиться) | `.env.example` |

Для debug-режиму потрібно скопіювати `appsettings.Template.json` → `appsettings.Development.json` і заповнити реальними значеннями.
Для docker-режиму потрібно скопіювати `.env.example` → `.env` у корені репо.

### Генерація RSA-ключа

Ключ генерується один раз і не повинен мінятись при кожному рестарті застосунку (інакше вже видані токени та закешований JWKS у клієнтів стануть невалідними). Наприклад:

```bash
openssl genrsa -out identity-private.pem 2048
```

Отриманий ключ (у PEM) — в `appsettings.Development.json`/`.env`, приватний файл `identity-private.pem` не комітиться.

---

## 🚀 Запуск та міграції БД

Таблиці створюються/оновлюються автоматично при старті застосунку (`Database.MigrateAsync()` перед сідингом адміна) — окремо застосовувати міграції вручну не потрібно.

**Docker:** `.env` у корені репо має бути заповнений (див. `.env.example`), далі:
```bash
docker compose up -d
```

**Debug:** `appsettings.Development.json` заповнений (див. вище), SQL Server піднятий хоча б з docker-compose:
```bash
docker compose up -d sql-server
```
Після цього сервіс запускається з IDE чи через `dotnet run` — міграції накотяться самі при старті.
