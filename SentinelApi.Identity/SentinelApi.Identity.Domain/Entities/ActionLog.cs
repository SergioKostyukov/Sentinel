using SentinelApi.Identity.Domain.Enums;

namespace SentinelApi.Identity.Domain.Entities;

public sealed class ActionLog
{
    public int Id { get; init; }
    public Guid AuthorId { get; init; }
    public Guid TargetId { get; init; }
    public ActionType ActionType { get; init; }
    public DateTime DateTime { get; init; }
    public string Description { get; init; } = null!;


    public User Author { get; set; } = null!;


    private ActionLog() { }
    public ActionLog(Guid authorId,
                     Guid targetId,
                     ActionType type,
                     string description)
    {
        AuthorId = authorId;
        TargetId = targetId;
        ActionType = type;
        DateTime = DateTime.UtcNow;
        Description = description;
    }
}
