using SentinelLib.Monitoring.SDK.Contracts;

namespace SentinelApi.Monitoring.Application.Models.CheckResult;

public sealed record CheckResultDTO(
    int Id,
    string CheckName,
    string CheckUrl,
    string CheckProbeType,
    string ServiceDefinitionName,
    string ServiceDefinitionUrl,
    string ServiceDefinitionNotificationEmails,
    DateTime CheckedAt,
    string TriggerType,
    string HealthStatus,
    ServiceCheckResponse? Response,
    string? ErrorMessage
);
