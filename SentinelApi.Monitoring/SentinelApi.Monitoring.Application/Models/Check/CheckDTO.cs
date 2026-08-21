namespace SentinelApi.Monitoring.Application.Models.Check;

public sealed record CheckDTO(
    int Id,
    int ServiceDefinitionId,
    string Name,
    string EndpointUrl,
    string Description,
    int ProbeType,
    bool IsEnabled
);
