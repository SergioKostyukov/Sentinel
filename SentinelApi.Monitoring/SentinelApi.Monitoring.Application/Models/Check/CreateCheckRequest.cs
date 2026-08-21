namespace SentinelApi.Monitoring.Application.Models.Check;

public sealed record CreateCheckRequest(
    int ServiceDefinitionId,
    string Name,
    string EndpointUrl,
    string Description,
    int ProbeType,
    bool IsEnabled
);
