using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SentinelApi.Monitoring.Application.Filters;
using SentinelApi.Monitoring.Application.Helpers;
using SentinelApi.Monitoring.Application.Interfaces;
using SentinelApi.Monitoring.Application.Interfaces.Infrastructure;
using SentinelApi.Monitoring.Application.Mappers;
using SentinelApi.Monitoring.Application.Models.CheckResult;
using SentinelApi.Monitoring.Domain.Entities;
using SentinelApi.Monitoring.Domain.Enums;

namespace SentinelApi.Monitoring.Application.Services;

public class CheckResultService(
    IEmailSenderService emailSender,
    ISentinelMonitoringDbContext dbContext) : ICheckResultService
{
    private readonly ISentinelMonitoringDbContext _dbContext = dbContext;
    private readonly IEmailSenderService _emailSender = emailSender;

    public async Task<CheckResultDTO> GetAsync(int id, CancellationToken ct)
    {
        var check = await _dbContext.CheckResults
            .AsNoTracking()
            .Include(x => x.Check)
                .ThenInclude(x => x.ServiceDefinition)
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync(ct)
            ?? throw new KeyNotFoundException($"There are no CheckResult with ID {id}");

        return check.ToDto();
    }

    public async Task<GetCheckResultListResponse> GetListAsync(GetCheckResultListRequest request, CancellationToken ct)
    {
        var checkResults = _dbContext.CheckResults
           .AsNoTracking()
           .Include(c => c.Check)
                .ThenInclude(c => c.ServiceDefinition)
           .SearchByParam(request.SearchParam)
           .SearchByDate(request.SearchDate);

        var totalCount = await checkResults.CountAsync(ct);

        var checkResultList = await checkResults
            .OrderByDescending(x => x.CheckedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return new GetCheckResultListResponse(
            TotalCount: totalCount,
            CheckResultList: checkResultList.ToViewDtoList()
        );
    }

    public async Task SendDailyReportAsync(DateTime date, CancellationToken ct)
    {
        var start = date.Date;
        var end = start.AddDays(1);

        var results = await _dbContext.CheckResults
            .AsNoTracking()
            .Include(x => x.Check)
                .ThenInclude(x => x.ServiceDefinition)
            .Where(x =>
                x.CheckedAt >= start &&
                x.CheckedAt < end)
            .ToListAsync(ct);

        var userReports = new Dictionary<string, List<CheckResult>>(StringComparer.OrdinalIgnoreCase);

        foreach (var result in results)
        {
            var emails = ParseEmails(result.Check.ServiceDefinition.NotificationEmails);

            foreach (var email in emails)
            {
                if (!userReports.TryGetValue(email, out var checks))
                {
                    checks = [];
                    userReports[email] = checks;
                }

                checks.Add(result);
            }
        }

        foreach (var report in userReports)
        {
            var email = report.Key;
            var checks = report.Value;

            var body = BuildReport(checks);

            await _emailSender.SendEmailAsync(email, "Daily monitoring", body, ct);
        }
    }

    /// <summary>
    /// Парсинг рядка електронних адрес, розділених комами, і повернення масиву унікальних адрес.
    /// </summary>
    private static string[] ParseEmails(string? emails)
    {
        if (string.IsNullOrWhiteSpace(emails))
            return [];

        return emails
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .Where(x => x.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    /// <summary>
    /// Формування звіту у вигляді HTML для email-розсилки на основі результатів перевірки.
    /// </summary>
    private static string BuildReport(List<CheckResult> results)
    {
        var sb = new StringBuilder();

        sb.AppendLine("""
        <html>
        <body style="font-family: Segoe UI, Arial, sans-serif; font-size: 14px; color: #222;">
            <h2 style="margin-bottom: 4px;">Daily service check report</h2>
        """);

        var orderedGroups = results
            .GroupBy(x => x.Check.ServiceDefinition.Name)
            .OrderBy(g => g.Key);

        foreach (var group in orderedGroups)
        {
            var hasIssues = group.Any(r => r.HealthStatus != ServiceHealthStatus.Healthy);
            var borderColor = hasIssues ? "#d93025" : "#188038";

            sb.AppendLine($"""
            <div style="border-left: 4px solid {borderColor}; padding: 8px 16px; margin: 16px 0; background: #fafafa;">
                <h3 style="margin: 0 0 8px 0;">{Encode(group.Key)}</h3>
            """);

            foreach (var result in group)
            {
                var isHealthy = result.HealthStatus == ServiceHealthStatus.Healthy;
                var statusColor = isHealthy ? "#188038" : "#d93025";
                var statusText = result.HealthStatus.GetEnumDescription();

                sb.AppendLine($"""
                <div style="margin: 8px 0; padding: 8px; background: #fff; border: 1px solid #e0e0e0; border-radius: 4px;">
                    <div><b>{Encode(result.Check.Name)}</b> — <span style="color: {statusColor}; font-weight: bold;">{Encode(statusText)}</span></div>
                    <div style="color: #666; font-size: 12px;">
                        {Encode(result.Check.ProbeType.GetEnumDescription())} · {result.CheckedAt:yyyy-MM-dd HH:mm:ss} UTC
                    </div>
                """);

                if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                {
                    sb.AppendLine($"""<div style="color: #d93025; margin-top: 4px;">⚠ {Encode(result.ErrorMessage)}</div>""");
                }

                if (!isHealthy && !string.IsNullOrWhiteSpace(result.ResponseJson))
                {
                    sb.AppendLine($"""
                    <details style="margin-top: 4px;">
                        <summary style="cursor: pointer; color: #666;">Details</summary>
                        <pre style="white-space: pre-wrap; font-size: 12px; background: #f5f5f5; padding: 8px; border-radius: 4px;">{Encode(result.ResponseJson)}</pre>
                    </details>
                    """);
                }

                sb.AppendLine("</div>");
            }

            sb.AppendLine("</div>");
        }

        sb.AppendLine("""
            </body>
        </html>
        """);

        return sb.ToString();

        static string Encode(string? value) => WebUtility.HtmlEncode(value ?? string.Empty);
    }
}
