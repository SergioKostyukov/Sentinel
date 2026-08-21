using Microsoft.EntityFrameworkCore;
using SentinelApi.Monitoring.Application.Filters;
using SentinelApi.Monitoring.Application.Interfaces;
using SentinelApi.Monitoring.Application.Interfaces.Infrastructure;
using SentinelApi.Monitoring.Application.Mappers;
using SentinelApi.Monitoring.Application.Models.History;
using SentinelApi.Monitoring.Domain.Entities;
using SentinelApi.Monitoring.Domain.Enums;

namespace SentinelApi.Monitoring.Application.Services;

public class HistoryService(ISentinelMonitoringDbContext dbContext) : IHistoryService
{
    private readonly ISentinelMonitoringDbContext _dbContext = dbContext;

    public async Task<GetActionLogListResponse> GetListAsync(GetActionLogListRequest request, CancellationToken ct)
    {
        var actionLogs = _dbContext.ActionLogs
            .AsQueryable();

        actionLogs = actionLogs.SearchByParam(request.SearchParam);

        var totalCount = await actionLogs.CountAsync(ct);

        var actionLogList = await actionLogs
            .AsNoTracking()
            .OrderByDescending(x => x.DateTime)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return new GetActionLogListResponse
        {
            Actions = actionLogList.ToViewDtoList(),
            TotalCount = totalCount
        };
    }

    public async Task SaveServiceDefinitionCreateActionLogAsync(string userId, string userLogin, string targetId, string targetName, string description, CancellationToken ct)
       => await SaveActionLogAsync(userId, userLogin, targetId, targetName, ActionType.ServiceDefinitionCreate, description, ct);

    public async Task SaveCheckCreateActionLogAsync(string userId, string userLogin, string targetId, string targetName, string description, CancellationToken ct)
       => await SaveActionLogAsync(userId, userLogin, targetId, targetName, ActionType.CheckCreate, description, ct);

    public async Task SaveServiceCheckTriggerActionLogAsync(string userId, string userLogin, string targetId, string targetName, string description, CancellationToken ct)
       => await SaveActionLogAsync(userId, userLogin, targetId, targetName, ActionType.ServiceCheckTrigger, description, ct);

    private async Task SaveActionLogAsync(string userId, string userLogin, string targetId, string targetName, ActionType type, string description, CancellationToken ct)
    {
        var actionLog = new ActionLog(userId, userLogin, targetId, targetName, type, description);

        await _dbContext.ActionLogs.AddAsync(actionLog, ct);
        await _dbContext.SaveChangesAsync(ct);
    }
}
