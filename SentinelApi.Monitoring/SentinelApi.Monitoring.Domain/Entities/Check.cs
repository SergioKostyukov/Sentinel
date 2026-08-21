using SentinelApi.Monitoring.Domain.Enums;

namespace SentinelApi.Monitoring.Domain.Entities;

/// <summary>
/// Одна окрема перевірка для сервісу.
/// </summary>
public sealed class Check
{
    public int Id { get; init; }
    public int ServiceDefinitionId { get; init; }
    public string Name { get; private set; } = null!;
    public string EndpointUrl { get; private set; } = null!; // відносний шлях до ендпоінту перевірки, наприклад: /api/health
    public string Description { get; private set; } = null!;
    public ProbeType ProbeType { get; private set; }
    public bool IsEnabled { get; private set; }


    public ServiceDefinition ServiceDefinition { get; set; } = default!;


    private Check() { }
    public Check(
        int serviceDefinitionId,
        string name,
        string endpointUrl,
        string description,
        ProbeType probeType,
        bool isEnabled)
    {
        ServiceDefinitionId = serviceDefinitionId;
        Name = name;
        EndpointUrl = endpointUrl;
        Description = description;
        ProbeType = probeType;
        IsEnabled = isEnabled;
    }


    public void Update(
        string name,
        string endpointUrl,
        string description,
        ProbeType probeType,
        bool isEnabled)
    {
        Name = name;
        EndpointUrl = endpointUrl;
        Description = description;
        ProbeType = probeType;
        IsEnabled = isEnabled;
    }
}
