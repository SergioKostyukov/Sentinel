using System.Security.Cryptography;

namespace SentinelApi.Identity.Application.Interfaces.Infrastructure;

public interface IJwtKeyProvider
{
    /// <summary>
    /// Повертає RSA-ключ, яким підписуються токени доступу.
    /// </summary>
    RSA GetKey();
}
