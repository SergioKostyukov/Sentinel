namespace SentinelApi.Monitoring.Models.History;

public sealed record GetActionHistoryListModel(
    string? SearchParam,
    int PageSize,
    int Page
);
