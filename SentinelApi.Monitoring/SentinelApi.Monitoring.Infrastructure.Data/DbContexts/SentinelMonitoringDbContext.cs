using Microsoft.EntityFrameworkCore;
using SentinelApi.Monitoring.Application.Interfaces.Infrastructure;
using SentinelApi.Monitoring.Domain.Entities;

namespace SentinelApi.Monitoring.Infrastructure.Data.DbContexts;

public class SentinelMonitoringDbContext(DbContextOptions<SentinelMonitoringDbContext> options) : DbContext(options), ISentinelMonitoringDbContext
{
    public DbSet<ActionLog> ActionLogs { get; set; }
    public DbSet<Check> Checks { get; set; }
    public DbSet<CheckResult> CheckResults { get; set; }
    public DbSet<ServiceDefinition> ServiceDefinitions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var assembly = typeof(SentinelMonitoringDbContext).Assembly;

        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
    }
}
