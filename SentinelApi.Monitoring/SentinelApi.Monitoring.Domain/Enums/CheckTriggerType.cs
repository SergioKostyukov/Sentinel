using System.ComponentModel;

namespace SentinelApi.Monitoring.Domain.Enums;

/// <summary>
/// Тип тригера перевірки сервісу.
/// </summary>
public enum CheckTriggerType
{
    [Description("Запланований")]
    Scheduled = 0,

    [Description("Ручний")]
    Manual = 1
}
