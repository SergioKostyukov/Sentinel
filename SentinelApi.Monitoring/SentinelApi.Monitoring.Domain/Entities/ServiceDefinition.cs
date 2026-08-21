namespace SentinelApi.Monitoring.Domain.Entities;

/// <summary>
/// Сервіс, який підлягає моніторингу.
/// </summary>
public sealed class ServiceDefinition
{
    public int Id { get; init; }
    public string Name { get; private set; } = null!;
    public string Url { get; private set; } = null!; // базовий URL сервісу, наприклад: https://example.com
    public string NotificationEmails { get; private set; } = null!; // список email-адрес через кому, на які будуть надсилатися повідомлення про стан сервісу
    public string Description { get; private set; } = null!;


    public ICollection<Check> Checks { get; set; } = [];


    private ServiceDefinition() { }
    public ServiceDefinition(
        string name,
        string url,
        string notificationEmails,
        string description)
    {
        Name = name;
        Url = url;
        NotificationEmails = notificationEmails;
        Description = description;
    }


    public void Update(
        string name,
        string url,
        string notificationEmails,
        string description)
    {
        Name = name;
        Url = url;
        NotificationEmails = notificationEmails;
        Description = description;
    }
}
