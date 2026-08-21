# Правила стилю комітів — Sentinel

Базується на Conventional Commits — розпізнається рецензентами й GitHub автоматично підсвічує теги.

## Формат

```
<type>(<scope>): <короткий опис у наказовому способі>

<тіло — необов'язково, що і чому>
```

**Приклад:**
```
feat(identity): add JWT token issuance on login

Implements access + refresh token pair generation.
Refresh tokens stored with 30-day expiry.
```

---

## Type

| Type | Коли використовувати |
|---|---|
| `feat` | Новий функціонал (новий контролер, бібліотека або фіча) |
| `fix` | Виправлення бага |
| `refactor` | Зміна структури коду без зміни поведінки |
| `docs` | Зміни лише в документації (README, коментарі) |
| `chore` | Залежності, конфіги, .gitignore |

---

## Scope

Вказує, якого проєкту/модуля стосується зміна:

- `api-identity` — SentinelApi.Monitoring.Identity
- `api-monitoring` — SentinelApi.Monitoring
- `api-monitoring-test` — SentinelApi.Monitoring.Test
- `lib-identity-security` — SentinelLib.Monitoring.Identity.Security
- `lib-monitoring-sdk` — SentinelLib.Monitoring.SDK
- `lib-monitoring-logging-nlog` — SentinelLib.Monitoring.Logging.NLog
- `ui` — SentinelUI
- `docker` — docker-compose / Dockerfile
- `ci` — pipeline

---

## Конфігурація: appsettings vs env

Застосунок підтримує два режими запуску, і кожен має власне джерело чутливих даних — вони **не змішуються**: значення для docker-режиму (внутрішні DNS-імена контейнерів, наприклад `http://identity:8080`) ніколи не потрапляють у файли debug-режиму, і навпаки (`localhost`-адреси).

| Режим запуску | Джерело чутливих даних | Шаблон (комітиться, без значень) |
|---|---|---|
| Debug (`dotnet run` / IDE) | `appsettings.Development.json` — **не комітиться** | `appsettings.Template.json` |
| Docker (`docker compose`) | `.env` — **не комітиться** | `.env.example` |

`appsettings.json` — мінімальний нечутливий конфіг, однаковий для обох режимів, **комітиться** (`AllowedHosts`, `Jwt:AccessTokenLifetime` тощо).

Секрети (`Jwt:PrivateKey`, `Seed:AdminPassword`, паролі БД) зберігаються **лише** в `appsettings.Development.json`/`.env` — інструмент `dotnet user-secrets` не використовується, щоб не мати третього паралельного джерела значень.

Кожен сервіс, що читає `IConfiguration`, має свій `appsettings.Template.json` з debug-орієнтованими плейсхолдерами (`localhost`, проброшені порти) — навіть для не-секретних, але environment-залежних ключів (issuer/audience тощо), щоб було видно, які ключі обов'язкові для локального запуску.
