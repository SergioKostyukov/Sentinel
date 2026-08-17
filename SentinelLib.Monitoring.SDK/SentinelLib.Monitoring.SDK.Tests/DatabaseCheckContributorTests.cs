using Microsoft.EntityFrameworkCore;
using SentinelLib.Monitoring.SDK.Enums;
using SentinelLib.Monitoring.SDK.Services;
using Xunit;

namespace SentinelLib.Monitoring.SDK.Tests;

public sealed class DatabaseCheckContributorTests
{
    private sealed class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options);

    [Fact]
    public async Task CheckAsync_WhenDatabaseIsReachable_ReturnsHealthy()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        await using var dbContext = new TestDbContext(options);

        var sut = new DatabaseCheckContributor<TestDbContext>(dbContext, "MainDatabase");

        var result = await sut.CheckAsync(CancellationToken.None);

        Assert.Equal("MainDatabase", result.Name);
        Assert.Equal(HealthStatus.Healthy, result.Status);
    }

    [Fact]
    public async Task CheckAsync_WhenConnectionCheckThrows_ReturnsUnhealthyWithoutPropagating()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var dbContext = new TestDbContext(options);
        await dbContext.DisposeAsync();

        var sut = new DatabaseCheckContributor<TestDbContext>(dbContext, "MainDatabase");

        var result = await sut.CheckAsync(CancellationToken.None);

        Assert.Equal("MainDatabase", result.Name);
        Assert.Equal(HealthStatus.Unhealthy, result.Status);
        Assert.Contains(result.Details, d => d.Key == "Error");
    }
}
