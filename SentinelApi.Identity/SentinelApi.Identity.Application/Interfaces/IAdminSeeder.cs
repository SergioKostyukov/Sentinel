namespace SentinelApi.Identity.Application.Interfaces;

public interface IAdminSeeder
{
    /// <summary>
    /// Створює першого (admin) користувача, якщо в системі ще жодного немає.
    /// Ідемпотентно.
    /// </summary>
    Task SeedAsync(string login, string email, string password, CancellationToken ct);
}
