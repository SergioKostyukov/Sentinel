using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SentinelApi.Monitoring.Application.Interfaces;
using SentinelApi.Monitoring.Application.Interfaces.Infrastructure;
using SentinelApi.Monitoring.Domain.Entities;
using SentinelApi.Monitoring.Domain.Enums;
using SentinelLib.Monitoring.SDK.Contracts;
using SentinelLib.Monitoring.SDK.Enums;
using SentinelLib.Monitoring.SDK.Security;

namespace SentinelApi.Monitoring.Application.Services;

public sealed class CheckExecutor(
    IHttpClientFactory httpClientFactory,
    ISentinelMonitoringDbContext dbContext,
    IOptions<SentinelMonitoringOptions> options) : ICheckExecutor
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly ISentinelMonitoringDbContext _dbContext = dbContext;
    private readonly string _apiKey = options.Value.ApiKey;

    public async Task<CheckResult> ExecuteManualAsync(int checkId, CancellationToken ct)
        => await ExecuteAsync(checkId, CheckTriggerType.Manual, ct);

    public async Task<CheckResult> ExecuteScheduleAsync(int checkId, CancellationToken ct)
        => await ExecuteAsync(checkId, CheckTriggerType.Scheduled, ct);

    private async Task<CheckResult> ExecuteAsync(int checkId, CheckTriggerType triggerType, CancellationToken ct)
    {
        var check = await _dbContext.Checks
            .Include(x => x.ServiceDefinition)
            .FirstAsync(x => x.Id == checkId, ct);

        var result = new CheckResult(check.Id, triggerType);

        try
        {
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(180);

            using var request = new HttpRequestMessage(HttpMethod.Get, check.ServiceDefinition.Url + check.EndpointUrl);

            request.Headers.Add(SentinelHeaders.ApiKey, _apiKey);

            using var httpResponse = await client.SendAsync(request, ct);
            httpResponse.EnsureSuccessStatusCode();

            var response = await httpResponse.Content.ReadFromJsonAsync<ServiceCheckResponse>(ct)
                ?? throw new InvalidOperationException("Failed to get service check response");

            var healthStatus = GetServiceHealthStatus(response);

            result.Success(healthStatus, JsonSerializer.Serialize(response));
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            result.Failure(ServiceHealthStatus.Unhealthy, "Sentinel API key mismatch or missing.");
        }
        catch (Exception ex)
        {
            result.Failure(ServiceHealthStatus.Unhealthy, ex.Message);
        }

        await _dbContext.CheckResults.AddAsync(result, ct);
        await _dbContext.SaveChangesAsync(ct);

        return result;
    }

    /// <summary>
    /// Визначення статусу сервісу на основі відповіді перевірки.
    /// </summary>
    private static ServiceHealthStatus GetServiceHealthStatus(ServiceCheckResponse response)
    {
        if (response.Components.Any(x => x.Status == HealthStatus.Unhealthy))
            return ServiceHealthStatus.Degraded;

        if (response.Components.All(x => x.Status == HealthStatus.Unhealthy))
            return ServiceHealthStatus.Unhealthy;

        return ServiceHealthStatus.Healthy;
    }
}
