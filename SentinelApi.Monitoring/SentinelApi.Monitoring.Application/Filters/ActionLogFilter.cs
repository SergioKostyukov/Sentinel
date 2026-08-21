using SentinelApi.Monitoring.Domain.Entities;

namespace SentinelApi.Monitoring.Application.Filters;

internal static class ActionLogFilter
{
    internal static IQueryable<ActionLog> SearchByParam(this IQueryable<ActionLog> actionLogs, string? searchParameter)
    {
        if (string.IsNullOrWhiteSpace(searchParameter))
        {
            return actionLogs;
        }

        searchParameter = searchParameter.ToLower();

        return actionLogs.Where(actionLog =>
            actionLog.UserLogin.ToLower().Contains(searchParameter) ||
            actionLog.TargetName.ToLower().Contains(searchParameter) ||
            actionLog.ActionType.ToString().ToLower().Contains(searchParameter));
    }
}
