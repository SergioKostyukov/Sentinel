using Microsoft.EntityFrameworkCore;
using SentinelApi.Identity.Application.Interfaces;
using SentinelApi.Identity.Infrastructure.Data.DbContexts;

namespace SentinelApi.Identity.Extensions;

internal static class AppInitialExtensions
{
    /// <summary>
    /// Застосовує незастосовані EF Core міграції, створюючи базу даних за потреби.
    /// </summary>
    internal static async Task MigrateDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

        await dbContext.Database.MigrateAsync();
    }

    internal static async Task SeedAdminUserAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<IAdminSeeder>();

        var adminLogin = GetRequiredConfigValue(app.Configuration, "Seed:AdminLogin");
        var adminEmail = GetRequiredConfigValue(app.Configuration, "Seed:AdminEmail");
        var adminPassword = GetRequiredConfigValue(app.Configuration, "Seed:AdminPassword");

        await seeder.SeedAsync(adminLogin, adminEmail, adminPassword, CancellationToken.None);
    }

    private static string GetRequiredConfigValue(IConfiguration configuration, string key)
    {
        var value = configuration[key];

        return string.IsNullOrWhiteSpace(value)
            ? throw new InvalidOperationException($"{key} is not configured.")
            : value;
    }
}
