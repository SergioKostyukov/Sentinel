using SentinelApi.Monitoring.Domain.Enums;

namespace SentinelApi.Monitoring.Domain.Entities;

/// <summary>
/// Лог дій користувачів сервісу.
/// </summary>
public sealed class ActionLog
{
    public int Id { get; init; }
    public string UserId { get; init; } = null!;
    public string UserLogin { get; init; } = null!; // логін користувача, який виконав дію (для зручності відображення в історії дій)
    public string TargetId { get; init; } = null!;
    public string TargetName { get; init; } = null!; // назва об'єкта, над яким виконувалась дія (для зручності відображення в історії дій)
    public ActionType ActionType { get; init; }
    public DateTime DateTime { get; init; }
    public string Description { get; init; } = null!; // зазвичай зберігається тіло запиту


    private ActionLog() { }
    public ActionLog(
        string userId,
        string userLogin,
        string targetId,
        string targetName,
        ActionType type,
        string description)
    {
        UserId = userId;
        UserLogin = userLogin;
        TargetId = targetId;
        TargetName = targetName;
        ActionType = type;
        DateTime = DateTime.Now;
        Description = description;
    }
}
