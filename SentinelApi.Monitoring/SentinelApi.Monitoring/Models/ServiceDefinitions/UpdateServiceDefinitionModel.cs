namespace SentinelApi.Monitoring.Models.ServiceDefinition;

public sealed record UpdateServiceDefinitionModel(
    int Id,
    string Name,
    string Url,
    string NotificationEmails,
    string Description
);
