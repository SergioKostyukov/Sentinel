namespace SentinelApi.Monitoring.Models.Check;

public sealed record UpdateCheckModel(
    int Id,
    string Name,
    string EndpointUrl,
    string Description,
    int ProbeType,
    bool IsEnabled
);
