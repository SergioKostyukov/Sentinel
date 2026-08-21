using SentinelApi.Monitoring.Application.Models.Option;

namespace SentinelApi.Monitoring.Application.Interfaces;

public interface IOptionService
{
    /// <summary>
    /// Отримання опцій зарестрованих сервісів.
    /// </summary>
    Task<List<OptionDTO>> GetServiceDefinitionsAsync(CancellationToken ct);
}
