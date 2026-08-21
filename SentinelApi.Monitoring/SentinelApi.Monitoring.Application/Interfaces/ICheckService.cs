using SentinelApi.Monitoring.Application.Models.Check;

namespace SentinelApi.Monitoring.Application.Interfaces;

public interface ICheckService
{
    /// <summary>
    /// Отримання назви перевірки за її Id
    /// </summary>
    Task<string> GetNameByIdAsync(int id, CancellationToken ct);

    /// <summary>
    /// Отримання детільної інформації про конфігурацію перевірки
    /// </summary>
    Task<CheckDTO> GetAsync(int id, CancellationToken ct);

    /// <summary>
    /// Отримання списку наявних конфігурацій перевірок
    /// </summary>
    Task<List<CheckViewDTO>> GetListAsync(CancellationToken ct);

    /// <summary>
    /// Створення конфігурації перевірки
    /// </summary>
    Task<int> CreateAsync(CreateCheckRequest request, CancellationToken ct);

    /// <summary>
    /// Оновлення конфігурації перевірки
    /// </summary>
    Task UpdateAsync(UpdateCheckRequest request, CancellationToken ct);
}
