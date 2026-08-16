# SentinelLib.Identity.Security

Бібліотека безпеки для сервісів Sentinel.
Використовується API сервісами SentinelApi для валідації JWT токенів, виданих сервісом SentinelApi.Identity.

---

## 🧩 Assignment

Бібліотека відповідає за:
- перевірку JWT токенів;
- налаштування Authentication;
- доступ до claims токена поточного користувача.

Генерація токенів виконується сервісом SentinelApi.Identity.

### Related Documentation

| Document | Description |
| --- | --- |
| [Project README](../../README.md) | README (архітектура і повна документація проекту) | 

## How To Use

### 1. Підключення бібліотеки

```csharp
<ProjectReference Include="..\..\SentinelApi.Identity.Security\SentinelApi.Identity.Security.csproj" />
```

### 2. Налаштувати JWT параметри

```json
{
  "Jwt": {
    "Issuer": "SA-IDENTITY-ADDRESS",
    "Audience": "SentinelApi"
  }
}
```

### 3. Реєстрація сервісів у Program.cs

```csharp
builder.Services.AddSentinelAuthentication(builder.Configuration, requireHttpsMetadata: !builder.Environment.IsDevelopment());
```

`requireHttpsMetadata` за замовчуванням `true` — лише для локальної розробки без TLS. У продакшн/докер-мережі значення завжди має лишатись `true`.

### 4. Підключення middleware

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

### 5. Використання авторизації

```csharp
[Authorize]
[ApiController]
[Route("api/example")]
public sealed class ExampleController : ControllerBase
{
}
```

## 👤 Current User Accessor

Бібліотека автоматично реєструє `ICurrentUserAccessor`, який надає доступ до інформації про поточного користувача, отриманої з JWT токена.

Використання:

```csharp
public sealed class ExampleService(ICurrentUserAccessor currentUser)
{
    private readonly ICurrentUserAccessor _currentUser = currentUser;

    public Task ExecuteAsync()
    {
        var userId = _currentUser.UserId;
        var login = _currentUser.Login;
        var email = _currentUser.Email;

        return Task.CompletedTask;
    }
}
```

## 🌐 Identity Endpoints

Бібліотека використовує наступні endpoint сервісу SentinelApi.Identity:

```csharp
GET /.well-known/openid-configuration
GET /.well-known/jwks.json
```

## 🔄 Authentication Flow

- Користувач виконує Login через SentinelApi.Identity;
- Identity генерує JWT токен;
- Клієнт передає JWT токен до API сервісу;
- Security Library отримує OpenID Metadata;
- Security Library отримує JWKS ключ;
- JWT токен валідується.
