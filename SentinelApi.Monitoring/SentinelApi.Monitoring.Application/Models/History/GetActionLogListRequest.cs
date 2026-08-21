namespace SentinelApi.Monitoring.Application.Models.History;

public record GetActionLogListRequest(
    string? SearchParam,
    int PageSize,
    int Page
);
