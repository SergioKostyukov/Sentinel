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
