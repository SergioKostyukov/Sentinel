using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using SentinelApi.Identity.Application.Interfaces.Infrastructure;

namespace SentinelApi.Identity.Infrastructure.Security;

/// <summary>
/// Завантажує RSA-ключ підпису один раз під час старту. Свідомо не перегенеровує ключ під час роботи —
/// сторонні сервіси кешують JWKS з /.well-known/jwks.json, тож зміна ключа інвалідувала б
/// уже видані токени та застарілі кеші при кожному перезапуску.
/// </summary>
public sealed class JwtKeyProvider : IJwtKeyProvider
{
    private readonly RSA _rsa;

    public JwtKeyProvider(IOptions<JwtIssuingOptions> options)
    {
        // Змінні середовища не можуть містити реальні переноси рядків, тому PEM у них зберігається з літеральним "\n" — нормалізуємо перед парсингом.
        var pem = options.Value.PrivateKey.Replace("\\n", "\n");

        _rsa = RSA.Create();
        _rsa.ImportFromPem(pem);
    }

    public RSA GetKey() => _rsa;
}
