using SentinelApi.Monitoring.Domain.Enums;

namespace SentinelApi.Monitoring.Domain.Entities;

/// <summary>
/// Результат перевірки сервісу.
/// </summary>
public class CheckResult
{
    public int Id { get; init; }
    public int CheckId { get; init; }
    public DateTime CheckedAt { get; init; }
    public CheckTriggerType TriggerType { get; init; }
    public ServiceHealthStatus HealthStatus { get; private set; }
    public string ResponseJson { get; private set; } = null!;
    public string? ErrorMessage { get; private set; }


    public Check Check { get; set; } = default!;


    private CheckResult() { }
    public CheckResult(
        int checkId,
        CheckTriggerType triggerType)
    {
        CheckId = checkId;
        CheckedAt = DateTime.Now;
        TriggerType = triggerType;
        HealthStatus = ServiceHealthStatus.Unknown;
    }


    public void Success(
        ServiceHealthStatus healthStatus,
        string responseJson)
    {
        HealthStatus = healthStatus;
        ResponseJson = responseJson;
        ErrorMessage = null;
    }

    public void Failure(
        ServiceHealthStatus healthStatus,
        string errorMessage)
    {
        HealthStatus = healthStatus;
        ResponseJson = string.Empty;
        ErrorMessage = errorMessage;
    }
}
