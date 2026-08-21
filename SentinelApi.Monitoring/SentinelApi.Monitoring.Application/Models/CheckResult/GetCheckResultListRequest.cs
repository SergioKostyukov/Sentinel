namespace SentinelApi.Monitoring.Application.Models.CheckResult;

public sealed record GetCheckResultListRequest(
    string? SearchParam,
    DateTime? SearchDate,
    int PageSize,
    int Page
);
