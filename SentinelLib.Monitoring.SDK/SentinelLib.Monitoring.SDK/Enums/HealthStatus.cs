using System.ComponentModel;

namespace SentinelLib.Monitoring.SDK.Enums;

/// <summary>
/// Стан функціонування компонента.
/// 0...9 - невідомий стан
/// 10...19 - коректне функціонування
/// 20 - помилкове функціонування
/// </summary>
public enum HealthStatus
{
    [Description("Unknown")]
    Unknown = 0,

    [Description("Healthy")]
    Healthy = 10,

    [Description("Unhealthy")]
    Unhealthy = 20
}
