namespace SentinelApi.Monitoring.Application.Models.ServiceDefinition;

public sealed record ServiceDefinitionDTO(
    int Id,
    string Name,
    string Url,
    string NotificationEmails,
    string Description,
    Dictionary<int, string> Checks
);
