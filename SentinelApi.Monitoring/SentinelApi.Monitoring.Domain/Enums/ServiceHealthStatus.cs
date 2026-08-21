using System.ComponentModel;

namespace SentinelApi.Monitoring.Domain.Enums;

/// <summary>
/// Стан функціонування сервісу.
/// 0...9 - невідомий стан
/// 10...19 - коректне функціонування
/// 20...29 - частково помилкове функціонування (частково коректне)
/// 30 - помилкове функціонування
/// </summary>
public enum ServiceHealthStatus
{
    [Description("Невідомий")]
    Unknown = 0,

    [Description("Коректний")]
    Healthy = 10,

    [Description("Частково помилковий")]
    Degraded = 20,

    [Description("Помилковий")]
    Unhealthy = 30
}
