using System.ComponentModel;

namespace SentinelApi.Monitoring.Domain.Enums;

/// <summary>
/// Тип перевірки сервісу.
/// </summary>
public enum ProbeType
{
    [Description("Health-check")]
    Health = 0,

    [Description("Snapshot-check")]
    Snapshot = 1,

    [Description("Certificate-check")]
    Certificate = 2
}
