namespace SentinelApi.Monitoring.Models.Check;

public sealed record CreateCheckModel(
    int ServiceDefinitionId,
    string Name,
    string EndpointUrl,
    string Description,
    int ProbeType,
    bool IsEnabled
);
