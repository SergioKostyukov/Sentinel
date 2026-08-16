using SentinelApi.Identity.Application.Interfaces.Infrastructure;
using SentinelApi.Identity.Domain.Entities;
using SentinelApi.Identity.Infrastructure.Data.DbContexts;

namespace SentinelApi.Identity.Infrastructure.Data.Repositories;

public sealed class ActionLogRepository(IdentityDbContext dbContext) : IActionLogRepository
{
    public async Task AddAsync(ActionLog actionLog, CancellationToken ct)
    {
        dbContext.ActionLogs.Add(actionLog);
        await dbContext.SaveChangesAsync(ct);
    }
}
