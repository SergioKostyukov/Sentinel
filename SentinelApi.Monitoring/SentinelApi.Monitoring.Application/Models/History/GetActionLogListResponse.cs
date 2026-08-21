namespace SentinelApi.Monitoring.Application.Models.History;

public class GetActionLogListResponse
{
    public List<ActionLogViewDTO>? Actions { get; set; }
    public int TotalCount { get; set; }
}

public record ActionLogViewDTO(
    int Id,
    string UserLogin,
    string TargetName,
    string ActionType,
    string DateTime,
    string Description
);
