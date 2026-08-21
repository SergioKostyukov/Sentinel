namespace SentinelApi.Monitoring.Models.CheckResult;

public sealed record GetCheckResultListModel(
    string? SearchParam,
    DateTime? SearchDate,
    int PageSize,
    int Page
);
