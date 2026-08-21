namespace SentinelApi.Monitoring.Domain.Exceptions;

/// <summary>
/// Виключення, що виникає при помилці запиту до зовнішнього сервісу SMTP.
/// </summary>
public class EmailSendException(string message, Exception? innerException = null) : Exception(message, innerException) { }
