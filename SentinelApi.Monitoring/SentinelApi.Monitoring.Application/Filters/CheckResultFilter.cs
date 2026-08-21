using SentinelApi.Monitoring.Domain.Entities;

namespace SentinelApi.Monitoring.Application.Filters;

internal static class CheckResultFilter
{
    internal static IQueryable<CheckResult> SearchByParam(this IQueryable<CheckResult> checkResults, string? searchParameter)
    {
        if (string.IsNullOrWhiteSpace(searchParameter))
        {
            return checkResults;
        }

        searchParameter = searchParameter.ToLower();

        return checkResults.Where(checkResult =>
            checkResult.Check.Name.ToLower().Contains(searchParameter) ||
            checkResult.Check.ServiceDefinition.Name.ToLower().Contains(searchParameter));
    }

    internal static IQueryable<CheckResult> SearchByDate(this IQueryable<CheckResult> checkResults, DateTime? searchDate)
    {
        if (!searchDate.HasValue)
            return checkResults;

        var start = searchDate.Value.Date;
        var end = start.AddDays(1);
        return checkResults.Where(x => x.CheckedAt >= start && x.CheckedAt < end);
    }
}
