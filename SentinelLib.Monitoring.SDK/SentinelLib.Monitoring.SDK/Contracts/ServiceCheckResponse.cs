using SentinelLib.Monitoring.SDK.Enums;

namespace SentinelLib.Monitoring.SDK.Contracts;

/// <summary>
/// Загальний результат перевірки стану сервісу.
/// </summary>
/// <param name="CheckedAt">Час виконання перевірки.</param>
/// <param name="Components">Список перевірених компонентів сервісу.</param>
/// <param name="Description">Додатковий опис результату перевірки.</param>
public sealed record ServiceCheckResponse(
    DateTime CheckedAt,
    IReadOnlyCollection<ServiceComponentCheck> Components,
    string? Description = null
);

/// <summary>
/// Результат перевірки окремого компонента сервісу.
/// </summary>
/// <param name="Name">Назва компонента.</param>
/// <param name="Status">Поточний стан компонента.</param>
/// <param name="Details">Додаткові параметри та метрики перевірки.</param>
/// <param name="Description">Додатковий опис компонента.</param>
public sealed record ServiceComponentCheck(
    string Name,
    HealthStatus Status,
    IReadOnlyCollection<CheckDetail> Details,
    string? Description = null
);

/// <summary>
/// Додаткова інформація про результат перевірки.
/// </summary>
/// <param name="Key">Назва параметра.</param>
/// <param name="Value">Значення параметра.</param>
/// <param name="ValueType">Тип значення параметра.</param>
/// <param name="Description">Опис параметра.</param>
public sealed record CheckDetail(
    string Key,
    string Value,
    CheckDetailType ValueType = CheckDetailType.Text,
    string? Description = null
);
