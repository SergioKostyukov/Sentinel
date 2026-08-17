namespace SentinelLib.Monitoring.SDK.Security;

/// <summary>
/// Налаштування доступу до Sentinel monitoring endpoint.
/// </summary>
public sealed class SentinelMonitoringOptions
{
    /// <summary>
    /// API ключ, який використовується Sentinel для авторизації запитів.
    /// </summary>
    public string ApiKey { get; set; } = null!;
}
