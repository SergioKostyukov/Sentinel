using System.Text.Json;
using SentinelApi.Monitoring.Application.Helpers;
using SentinelApi.Monitoring.Application.Models.CheckResult;
using SentinelApi.Monitoring.Domain.Entities;
using SentinelLib.Monitoring.SDK.Contracts;

namespace SentinelApi.Monitoring.Application.Mappers;

internal static class CheckResultMapper
{
    internal static CheckResultDTO ToDto(this CheckResult result)
    {
        ServiceCheckResponse? response = null;

        if (!string.IsNullOrWhiteSpace(result.ResponseJson))
        {
            response = JsonSerializer.Deserialize<ServiceCheckResponse>(result.ResponseJson);
        }

        return new CheckResultDTO(
            Id: result.Id,
            CheckName: result.Check.Name,
            CheckUrl: result.Check.EndpointUrl,
            CheckProbeType: result.Check.ProbeType.GetEnumDescription(),
            ServiceDefinitionName: result.Check.ServiceDefinition.Name,
            ServiceDefinitionUrl: result.Check.ServiceDefinition.Url,
            ServiceDefinitionNotificationEmails: result.Check.ServiceDefinition.NotificationEmails,
            CheckedAt: result.CheckedAt,
            TriggerType: result.TriggerType.GetEnumDescription(),
            HealthStatus: result.HealthStatus.GetEnumDescription(),
            Response: response,
            ErrorMessage: result.ErrorMessage
        );
    }

    internal static List<CheckResultViewDTO> ToViewDtoList(this List<CheckResult> checkResults)
        => [.. checkResults.Select(u => new CheckResultViewDTO(
            Id: u.Id,
            CheckId: u.CheckId,
            CheckName: u.Check.Name,
            ServiceDefinitionName: u.Check.ServiceDefinition.Name,
            CheckedAt: FormatDate(u.CheckedAt),
            TriggerType: u.TriggerType.GetEnumDescription(),
            HealthStatus: u.HealthStatus.GetEnumDescription()
        ))];

    internal static string FormatDate(DateTime? dt)
        => dt?.ToString("dd-MM-yyyy HH:mm:ss") ?? string.Empty;
}
