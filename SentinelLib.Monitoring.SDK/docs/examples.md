# Examples

[← Back to README](../README.md)

## Request Example

Health-check:

```http
GET /api/sentinel/check HTTP/1.1
Host: your-service.example.com
X-Sentinel-Key: ваш-ключ
```

Snapshot-check:

```http
GET /api/sentinel/snapshot HTTP/1.1
Host: your-service.example.com
X-Sentinel-Key: ваш-ключ
```

Заголовок обов'язковий для обох ендпоінтів — див. [Authorization](authorization.md).
Модель відповіді однакова для обох типів перевірки.

## Response Example

```json
{
  "checkedAt": "2026-07-14T10:00:00Z",
  "components": [
    {
      "name": "Database",
      "status": "Healthy",
      "details": [
        {
          "key": "Connection",
          "value": "OK",
          "valueType": "Text"
        }
      ]
    },
    {
      "name": "Background Jobs",
      "status": "Healthy",
      "details": [
        {
          "key": "LastExecution",
          "value": "2026-07-14T09:55:00",
          "valueType": "DateTime"
        }
      ]
    }
  ]
}
```

## Response Example — з помилкою авторизації

```http
HTTP/1.1 401 Unauthorized
```

Виникає, якщо заголовок `X-Sentinel-Key` відсутній або не збігається з налаштованим `SentinelMonitoringOptions.ApiKey`.
