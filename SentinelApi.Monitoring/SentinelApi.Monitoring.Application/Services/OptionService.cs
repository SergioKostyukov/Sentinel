using Microsoft.EntityFrameworkCore;
using SentinelApi.Monitoring.Application.Interfaces;
using SentinelApi.Monitoring.Application.Interfaces.Infrastructure;
using SentinelApi.Monitoring.Application.Models.Option;

namespace SentinelApi.Monitoring.Application.Services;

public sealed class OptionService(ISentinelMonitoringDbContext dbContext) : IOptionService
{
    private readonly ISentinelMonitoringDbContext _dbContext = dbContext;

    public async Task<List<OptionDTO>> GetServiceDefinitionsAsync(CancellationToken ct)
         => await _dbContext.ServiceDefinitions
            .AsNoTracking()
            .Select(x => new OptionDTO(x.Id, x.Name))
            .ToListAsync(ct);
}
