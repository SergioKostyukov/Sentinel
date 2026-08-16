namespace SentinelApi.Identity.Domain.Exceptions;

/// <summary>
/// Базовий клас для очікуваних винятків, які не є помилками системи і не потребують рівня Error.
/// </summary>
public abstract class AppException(string message, int statusCode) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}

public sealed class DuplicateUserException(string login)
    : AppException($"User with login '{login}' already exists.", 400);

public sealed class InvalidCredentialsException()
    : AppException("Invalid login or password.", 400);
