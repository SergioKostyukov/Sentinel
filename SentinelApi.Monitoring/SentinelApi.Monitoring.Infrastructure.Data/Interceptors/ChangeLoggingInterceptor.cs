using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SentinelApi.Monitoring.Domain.Entities;
using SentinelApi.Monitoring.Domain.Enums;
using SentinelLib.Identity.Security.CurrentUser;

namespace SentinelApi.Monitoring.Infrastructure.Data.Interceptors;

public sealed class ChangeLoggingInterceptor(ICurrentUserAccessor currentUserAccessor) : SaveChangesInterceptor
{
    private static readonly Dictionary<Type, ActionType> ActionTypesByEntity = new()
    {
        [typeof(Check)] = ActionType.CheckUpdate,
        [typeof(ServiceDefinition)] = ActionType.ServiceDefinitionUpdate
    };

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        AppendActionLogs(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken ct = default)
    {
        AppendActionLogs(eventData.Context);
        return base.SavingChangesAsync(eventData, result, ct);
    }

    private void AppendActionLogs(DbContext? context)
    {
        if (context is null) return;

        var modifiedEntries = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified && ActionTypesByEntity.ContainsKey(e.Entity.GetType()))
            .ToList();

        if (modifiedEntries.Count == 0) return;

        string userId = currentUserAccessor.UserId.ToString();
        string userLogin = currentUserAccessor.Login;

        foreach (var entry in modifiedEntries)
        {
            var actionLog = BuildActionLog(entry, userId, userLogin);
            if (actionLog is not null) context.Add(actionLog);
        }
    }

    private static ActionLog? BuildActionLog(EntityEntry entry, string userId, string userLogin)
    {
        var changes = entry.Properties
            .Where(p => p.IsModified && !Equals(p.OriginalValue, p.CurrentValue))
            .Select(p => new { Property = p.Metadata.Name, OldValue = p.OriginalValue, NewValue = p.CurrentValue })
            .ToList();

        if (changes.Count == 0) return null;

        var targetId = entry.Property("Id").CurrentValue?.ToString() ?? string.Empty;
        var targetName = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "Name")?.CurrentValue as string ?? targetId;
        var actionType = ActionTypesByEntity[entry.Entity.GetType()];
        var description = JsonSerializer.Serialize(changes);

        return new ActionLog(userId, userLogin, targetId, targetName, actionType, description);
    }
}
