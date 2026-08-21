namespace SentinelApi.Monitoring.Application.Interfaces.Infrastructure;

public interface IEmailSenderService
{
    /// <summary>
    /// Надсилає HTML-лист через SMTP.
    /// </summary>
    Task SendEmailAsync(string to, string subject, string body, CancellationToken ct = default);
}
