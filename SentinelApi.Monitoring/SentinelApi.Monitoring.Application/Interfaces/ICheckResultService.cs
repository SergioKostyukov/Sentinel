using SentinelApi.Monitoring.Application.Models.CheckResult;

namespace SentinelApi.Monitoring.Application.Interfaces;

public interface ICheckResultService
{
    /// <summary>
    /// Отримання детільної інформації про виконану перевірку
    /// </summary>
    Task<CheckResultDTO> GetAsync(int id, CancellationToken ct);

    /// <summary>
    /// Отримання списку виконаних перевірок
    /// </summary>
    Task<GetCheckResultListResponse> GetListAsync(GetCheckResultListRequest request, CancellationToken ct);

    /// <summary>
    /// Форування і відправка щоденного звіту статусу сервісів на електронну пошту
    /// </summary>
    Task SendDailyReportAsync(DateTime date, CancellationToken ct);
}
