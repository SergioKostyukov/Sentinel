namespace SentinelApi.Monitoring.Models.ServiceDefinition;

public sealed record CreateServiceDefinitionModel(
    string Name,
    string Url,
    string NotificationEmails,
    string Description
);
