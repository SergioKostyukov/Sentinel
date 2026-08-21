namespace SentinelApi.Monitoring.Application.Models.Check;

public sealed record UpdateCheckRequest(
    int Id,
    string Name,
    string EndpointUrl,
    string Description,
    int ProbeType,
    bool IsEnabled
);
