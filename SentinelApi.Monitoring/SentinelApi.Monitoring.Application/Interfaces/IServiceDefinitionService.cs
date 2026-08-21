using SentinelApi.Monitoring.Application.Models.ServiceDefinition;

namespace SentinelApi.Monitoring.Application.Interfaces;

public interface IServiceDefinitionService
{
    /// <summary>
    /// Отримання детільної інформації про конфігурацію сервісу
    /// </summary>
    Task<ServiceDefinitionDTO> GetAsync(int id, CancellationToken ct);

    /// <summary>
    /// Отримання списку наявних конфігурацій сервісів
    /// </summary>
    Task<List<ServiceDefinitionViewDTO>> GetListAsync(CancellationToken ct);

    /// <summary>
    /// Створення конфігурації сервісу
    /// </summary>
    Task<int> CreateAsync(CreateServiceDefinitionRequest request, CancellationToken ct);

    /// <summary>
    /// Оновлення конфігурації сервісу
    /// </summary>
    Task UpdateAsync(UpdateServiceDefinitionRequest request, CancellationToken ct);
}
