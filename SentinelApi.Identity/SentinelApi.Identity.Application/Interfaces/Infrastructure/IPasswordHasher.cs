namespace SentinelApi.Identity.Application.Interfaces.Infrastructure;

public interface IPasswordHasher
{
    /// <summary>
    /// Хешує пароль у вигляді, придатному для зберігання.
    /// </summary>
    string Hash(string password);

    /// <summary>
    /// Перевіряє, чи відповідає пароль збереженому хешу.
    /// </summary>
    bool Verify(string passwordHash, string password);
}
