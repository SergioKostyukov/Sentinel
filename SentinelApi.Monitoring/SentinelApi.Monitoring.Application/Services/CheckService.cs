using Microsoft.EntityFrameworkCore;
using SentinelApi.Monitoring.Application.Interfaces;
using SentinelApi.Monitoring.Application.Interfaces.Infrastructure;
using SentinelApi.Monitoring.Application.Mappers;
using SentinelApi.Monitoring.Application.Models.Check;
using SentinelApi.Monitoring.Domain.Enums;

namespace SentinelApi.Monitoring.Application.Services;

public class CheckService(ISentinelMonitoringDbContext dbContext) : ICheckService
{
    private readonly ISentinelMonitoringDbContext _dbContext = dbContext;

    public async Task<string> GetNameByIdAsync(int id, CancellationToken ct)
        => await _dbContext.Checks
            .Where(c => c.Id == id)
            .Select(c => c.Name)
            .FirstAsync(ct);

    public async Task<CheckDTO> GetAsync(int id, CancellationToken ct)
    {
        var check = await _dbContext.Checks
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync(ct)
            ?? throw new KeyNotFoundException($"There are no Check with ID {id}");

        return check.ToDto();
    }

    public async Task<List<CheckViewDTO>> GetListAsync(CancellationToken ct)
    {
        var checks = await _dbContext.Checks
            .AsNoTracking()
            .Include(c => c.ServiceDefinition)
            .OrderBy(x => x.ServiceDefinitionId)
                .ThenBy(x => x.IsEnabled)
                .ThenBy(x => x.Name)
            .ToListAsync(ct);

        return checks.ToViewDtoList();
    }

    public async Task<int> CreateAsync(CreateCheckRequest request, CancellationToken ct)
    {
        var check = request.ToEntity();

        await _dbContext.Checks.AddAsync(check, ct);
        await _dbContext.SaveChangesAsync(ct);

        return check.Id;
    }

    public async Task UpdateAsync(UpdateCheckRequest request, CancellationToken ct)
    {
        var check = await _dbContext.Checks
            .FirstOrDefaultAsync(q => q.Id == request.Id, ct)
            ?? throw new KeyNotFoundException($"There are no Check with ID {request.Id}");

        check.Update(
            name: request.Name,
            endpointUrl: request.EndpointUrl,
            description: request.Description,
            probeType: (ProbeType)request.ProbeType,
            isEnabled: request.IsEnabled
        );

        _dbContext.Checks.Update(check);
        await _dbContext.SaveChangesAsync(ct);
    }
}
