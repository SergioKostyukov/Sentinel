using Microsoft.EntityFrameworkCore;
using SentinelApi.Monitoring.Application.Interfaces;
using SentinelApi.Monitoring.Application.Interfaces.Infrastructure;
using SentinelApi.Monitoring.Application.Mappers;
using SentinelApi.Monitoring.Application.Models.ServiceDefinition;

namespace SentinelApi.Monitoring.Application.Services;

public class ServiceDefinitionService(ISentinelMonitoringDbContext dbContext) : IServiceDefinitionService
{
    private readonly ISentinelMonitoringDbContext _dbContext = dbContext;

    public async Task<ServiceDefinitionDTO> GetAsync(int id, CancellationToken ct)
    {
        var serviceDefinition = await _dbContext.ServiceDefinitions
            .Include(sd => sd.Checks)
            .Where(sd => sd.Id == id)
            .FirstOrDefaultAsync(ct)
            ?? throw new KeyNotFoundException($"There are no ServiceDefinition with ID {id}");

        return serviceDefinition.ToDto();
    }

    public async Task<List<ServiceDefinitionViewDTO>> GetListAsync(CancellationToken ct)
    {
        var serviceDefinitions = await _dbContext.ServiceDefinitions
            .Include(sd => sd.Checks)
            .AsNoTracking()
            .OrderBy(sd => sd.Name)
            .ToListAsync(ct);

        return serviceDefinitions.ToViewDtoList();
    }

    public async Task<int> CreateAsync(CreateServiceDefinitionRequest request, CancellationToken ct)
    {
        var serviceDefinition = request.ToEntity();

        await _dbContext.ServiceDefinitions.AddAsync(serviceDefinition, ct);
        await _dbContext.SaveChangesAsync(ct);

        return serviceDefinition.Id;
    }

    public async Task UpdateAsync(UpdateServiceDefinitionRequest request, CancellationToken ct)
    {
        var serviceDefinition = await _dbContext.ServiceDefinitions
            .FirstOrDefaultAsync(q => q.Id == request.Id, ct)
            ?? throw new KeyNotFoundException($"There are no ServiceDefinition with ID {request.Id}");

        serviceDefinition.Update(
            request.Name,
            request.Url,
            request.NotificationEmails,
            request.Description
        );

        _dbContext.ServiceDefinitions.Update(serviceDefinition);
        await _dbContext.SaveChangesAsync(ct);
    }
}
