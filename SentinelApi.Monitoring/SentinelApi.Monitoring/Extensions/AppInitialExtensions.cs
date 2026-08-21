using Microsoft.EntityFrameworkCore;
using SentinelApi.Monitoring.Infrastructure.Data.DbContexts;

namespace SentinelApi.Monitoring.Extensions;

internal static class AppInitialExtensions
{
    /// <summary>
    /// Застосовує незастосовані EF Core міграції, створюючи базу даних за потреби.
    /// </summary>
    internal static async Task MigrateDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SentinelMonitoringDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}
