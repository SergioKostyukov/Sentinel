using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SentinelLib.Monitoring.SDK.Security;
using Xunit;

namespace SentinelLib.Monitoring.SDK.Tests;

public sealed class SentinelApiKeyEndpointFilterTests
{
    private static readonly object NextResult = new();

    private static EndpointFilterDelegate Next { get; } = _ => ValueTask.FromResult<object?>(NextResult);

    [Fact]
    public async Task InvokeAsync_WhenApiKeyNotConfigured_ReturnsProblem500()
    {
        var context = CreateContext(configuredApiKey: null, providedHeader: null);
        var sut = new SentinelApiKeyEndpointFilter();

        var result = await sut.InvokeAsync(context, Next);

        var problem = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, problem.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_WhenHeaderMissing_ReturnsUnauthorized()
    {
        var context = CreateContext(configuredApiKey: "secret", providedHeader: null);
        var sut = new SentinelApiKeyEndpointFilter();

        var result = await sut.InvokeAsync(context, Next);

        var unauthorized = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status401Unauthorized, unauthorized.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_WhenHeaderDoesNotMatch_ReturnsUnauthorized()
    {
        var context = CreateContext(configuredApiKey: "secret", providedHeader: "wrong-key");
        var sut = new SentinelApiKeyEndpointFilter();

        var result = await sut.InvokeAsync(context, Next);

        var unauthorized = Assert.IsAssignableFrom<IStatusCodeHttpResult>(result);
        Assert.Equal(StatusCodes.Status401Unauthorized, unauthorized.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_WhenHeaderMatches_CallsNext()
    {
        var context = CreateContext(configuredApiKey: "secret", providedHeader: "secret");
        var sut = new SentinelApiKeyEndpointFilter();

        var result = await sut.InvokeAsync(context, Next);

        Assert.Same(NextResult, result);
    }

    private static EndpointFilterInvocationContext CreateContext(string? configuredApiKey, string? providedHeader)
    {
        var services = new ServiceCollection();
        services.AddSingleton(Options.Create(new SentinelMonitoringOptions { ApiKey = configuredApiKey! }));

        var httpContext = new DefaultHttpContext
        {
            RequestServices = services.BuildServiceProvider()
        };

        if (providedHeader is not null)
        {
            httpContext.Request.Headers[SentinelHeaders.ApiKey] = providedHeader;
        }

        return EndpointFilterInvocationContext.Create(httpContext);
    }
}
