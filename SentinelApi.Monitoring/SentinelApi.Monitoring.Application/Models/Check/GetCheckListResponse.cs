namespace SentinelApi.Monitoring.Application.Models.Check;

public sealed record GetCheckListResponse(
    List<CheckViewDTO> CheckList,
    int TotalCount
);

public sealed record CheckViewDTO(
    int Id,
    string Name,
    string ServiceName,
    string ProbeType,
    bool IsEnabled
);
