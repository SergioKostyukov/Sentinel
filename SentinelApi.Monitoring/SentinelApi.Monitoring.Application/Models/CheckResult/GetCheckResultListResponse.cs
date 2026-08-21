namespace SentinelApi.Monitoring.Application.Models.CheckResult;

public sealed record GetCheckResultListResponse(
    List<CheckResultViewDTO> CheckResultList,
    int TotalCount
);

public sealed record CheckResultViewDTO(
    int Id,
    int CheckId,
    string CheckName,
    string ServiceDefinitionName,
    string CheckedAt,
    string TriggerType,
    string HealthStatus
);
