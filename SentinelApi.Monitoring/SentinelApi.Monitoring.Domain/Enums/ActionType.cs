using System.ComponentModel;

namespace SentinelApi.Monitoring.Domain.Enums;

/// <summary>
/// Тип дії користувача сервісу.
/// </summary>
public enum ActionType
{
    [Description("Створення сервісу")]
    ServiceDefinitionCreate = 0,

    [Description("Оновлення сервісу")]
    ServiceDefinitionUpdate = 1,

    [Description("Створення перевірки")]
    CheckCreate = 5,

    [Description("Оновлення перевірки")]
    CheckUpdate = 6,

    [Description("Запуск перевірки")]
    ServiceCheckTrigger = 10
}
