namespace SentinelApi.Monitoring.Application.Models.ServiceDefinition;

public sealed record CreateServiceDefinitionRequest(
    string Name,
    string Url,
    string NotificationEmails,
    string Description
);
