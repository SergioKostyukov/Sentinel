namespace SentinelApi.Identity.Domain.Entities;

public sealed class User
{
    public Guid Id { get; init; }
    public string Login { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string PasswordHash { get; init; } = null!;
    public DateTime CreatedAtUtc { get; init; }


    public ICollection<ActionLog> ActionLogsAsAuthor { get; private set; } = [];


    private User() { }
    public User(string login,
                string email,
                string passwordHash)
    {
        Id = Guid.NewGuid();
        Login = login;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAtUtc = DateTime.UtcNow;
    }
}
