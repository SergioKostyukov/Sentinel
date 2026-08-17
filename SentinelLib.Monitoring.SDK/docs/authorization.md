# Authorization

[← Back to README](../README.md)

Endpoint'и `GET /api/sentinel/check` (health-check) та `GET /api/sentinel/snapshot` (snapshot-check)
захищені API-ключем. Запит без коректного заголовка `X-Sentinel-Key` поверне `401 Unauthorized`; 
якщо ключ не налаштовано на боці сервісу — `500 Internal Server Error`.
 
Використовується **один спільний ключ** для всіх сервісів, які опитує
Sentinel — той самий ключ, що налаштований на боці `SentinelApi.Monitoring`.
 
## Configuration
 
Через `appsettings.json` та секцію конфігурації:
 
```json
{
  "Sentinel": {
    "ApiKey": "ваш-ключ"
  }
}
```

## Захист власних endpoint'ів тим самим ключем
 
Якщо у сервісу є додаткові кастомні health/monitoring endpoint'и (minimal API),
які теж мають перевіряти Sentinel API-ключ:
 
```csharp
app.MapGet("/api/custom/health-extra", async () => Results.Ok(new { status = "ok" }))
   .RequireSentinelApiKey();
```
 
Для групи endpoint'ів:
 
```csharp
var group = app.MapGroup("/api/custom").RequireSentinelApiKey();
group.MapGet("/health-extra", ...);
group.MapGet("/another-check", ...);
```
 
Для endpoint'ів, зареєстрованих як MVC-контролери, `RequireSentinelApiKey()`
не застосовується — там потрібен окремий action filter/атрибут.

## Генерація ключа

Ключ — довільний криптографічно стійкий рядок, генерація не потребує коду з цієї бібліотеки чи сервісу:

```bash
openssl rand -hex 32
```
