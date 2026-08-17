using NSubstitute;
using SentinelLib.Monitoring.SDK.Abstractions;
using SentinelLib.Monitoring.SDK.Contracts;
using SentinelLib.Monitoring.SDK.Enums;
using SentinelLib.Monitoring.SDK.Services;
using Xunit;

namespace SentinelLib.Monitoring.SDK.Tests;

public sealed class ServiceCheckProviderTests
{
    [Fact]
    public async Task CheckAsync_AggregatesResultsFromAllContributors()
    {
        var first = FakeContributor("First", HealthStatus.Healthy);
        var second = FakeContributor("Second", HealthStatus.Unhealthy);

        var sut = new HealthCheckProvider([first, second]);

        var result = await sut.CheckAsync(CancellationToken.None);

        Assert.Equal(2, result.Components.Count);
        Assert.Contains(result.Components, c => c.Name == "First" && c.Status == HealthStatus.Healthy);
        Assert.Contains(result.Components, c => c.Name == "Second" && c.Status == HealthStatus.Unhealthy);
    }

    [Fact]
    public async Task CheckAsync_WithNoContributors_ReturnsEmptyComponents()
    {
        var sut = new HealthCheckProvider([]);

        var result = await sut.CheckAsync(CancellationToken.None);

        Assert.Empty(result.Components);
    }

    [Fact]
    public async Task CheckAsync_WhenOneContributorThrows_StillReturnsResultsFromOthers()
    {
        var healthy = FakeContributor("Healthy", HealthStatus.Healthy);

        var throwing = Substitute.For<IHealthCheckContributor>();
        throwing.CheckAsync(Arg.Any<CancellationToken>())
            .Returns<Task<ServiceComponentCheck>>(_ => throw new InvalidOperationException("boom"));

        var sut = new HealthCheckProvider([healthy, throwing]);

        var result = await sut.CheckAsync(CancellationToken.None);

        Assert.Equal(2, result.Components.Count);
        Assert.Contains(result.Components, c => c.Name == "Healthy" && c.Status == HealthStatus.Healthy);

        var failed = result.Components.Single(c => c.Name != "Healthy");
        Assert.Equal(HealthStatus.Unhealthy, failed.Status);
    }

    [Fact]
    public async Task CheckAsync_WhenContributorThrowsOperationCanceled_PropagatesInsteadOfSwallowing()
    {
        var throwing = Substitute.For<IHealthCheckContributor>();
        throwing.CheckAsync(Arg.Any<CancellationToken>())
            .Returns<Task<ServiceComponentCheck>>(_ => throw new OperationCanceledException());

        var sut = new HealthCheckProvider([throwing]);

        await Assert.ThrowsAsync<OperationCanceledException>(() => sut.CheckAsync(CancellationToken.None));
    }

    private static IHealthCheckContributor FakeContributor(string name, HealthStatus status)
    {
        var contributor = Substitute.For<IHealthCheckContributor>();
        contributor.CheckAsync(Arg.Any<CancellationToken>())
            .Returns(new ServiceComponentCheck(name, status, []));

        return contributor;
    }
}
