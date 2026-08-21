namespace SentinelApi.Monitoring.Application.Models.ServiceDefinition;

public sealed record UpdateServiceDefinitionRequest(
    int Id,
    string Name,
    string Url,
    string NotificationEmails,
    string Description
);
