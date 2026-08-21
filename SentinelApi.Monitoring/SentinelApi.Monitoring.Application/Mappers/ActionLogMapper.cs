using SentinelApi.Monitoring.Application.Helpers;
using SentinelApi.Monitoring.Application.Models.History;
using SentinelApi.Monitoring.Domain.Entities;

namespace SentinelApi.Monitoring.Application.Mappers;

internal static class ActionLogMapper
{
    internal static List<ActionLogViewDTO> ToViewDtoList(this List<ActionLog> actionLogs)
        => [.. actionLogs.Select(u => new ActionLogViewDTO(
            Id: u.Id,
            UserLogin: u.UserLogin,
            TargetName: u.TargetName,
            ActionType: u.ActionType.GetEnumDescription(),
            DateTime: FormatDate(u.DateTime),
            Description: u.Description ?? string.Empty
        ))];

    internal static string FormatDate(DateTime? dt)
        => dt?.ToString("dd-MM-yyyy HH:mm:ss") ?? string.Empty;
}
