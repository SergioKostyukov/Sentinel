namespace SentinelApi.Monitoring.Infrastructure.EmailSender;

public sealed class EmailSenderConfig
{
    public string Host { get; set; } = null!;
    public int Port { get; init; }
    public string UserName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}
