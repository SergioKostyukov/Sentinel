using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using NLog;
using SentinelApi.Monitoring.Application.Interfaces.Infrastructure;
using SentinelApi.Monitoring.Domain.Exceptions;

namespace SentinelApi.Monitoring.Infrastructure.EmailSender;

public class EmailSenderService(IOptions<EmailSenderConfig> settings) : IEmailSenderService
{
    private readonly Logger _logger = LogManager.GetLogger("SmtpEmailSender");
    private readonly EmailSenderConfig _emailSettings = settings.Value;

    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken ct = default)
    {
        try
        {
            var message = new MimeMessage();

            var from = new MailboxAddress(null, _emailSettings.Email);
            message.From.Add(from);

            var recipient = new MailboxAddress("", to);
            message.To.Add(recipient);

            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body,
                TextBody = "Ваш поштовий клієнт не підтримує HTML. Будь ласка, відкрийте лист у веб-версії."
            };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();

            // Перевірка сертифіката (якщо самопідписаний сертифікат)
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;

            // Вибір опцій для підключення залежно від порту
            SecureSocketOptions socketOptions = _emailSettings.Port switch
            {
                465 => SecureSocketOptions.SslOnConnect,        // SSL підключення
                587 => SecureSocketOptions.StartTls,            // STARTTLS підключення
                25 => SecureSocketOptions.StartTlsWhenAvailable, // TLS, якщо сервер підтримує (напр. без TLS для локального smtp4dev)
                _ => SecureSocketOptions.Auto
            };

            await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, socketOptions, ct);

            // Автентифікуємось лише якщо сервер її взагалі підтримує — сервери на кшталт
            // smtp4dev (локальна розробка) не оголошують SASL-механізми, і виклик
            // AuthenticateAsync без цієї перевірки кидає NotSupportedException.
            if (!string.IsNullOrEmpty(_emailSettings.UserName) && client.AuthenticationMechanisms.Count > 0)
            {
                await client.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password, ct);
            }

            await client.SendAsync(message);
            await client.DisconnectAsync(true, ct);

            _logger.Info("Email sent to {Recipient}.", to);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Email send failed to {Recipient}.", to);

            throw new EmailSendException("Unexpected email error.", ex);
        }
    }
}
