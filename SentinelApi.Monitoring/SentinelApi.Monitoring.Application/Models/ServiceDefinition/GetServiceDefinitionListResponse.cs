namespace SentinelApi.Monitoring.Application.Models.ServiceDefinition;

public sealed record GetServiceDefinitionListResponse(
    List<ServiceDefinitionViewDTO> ServiceDefinitionList,
    int TotalCount
);

public sealed record ServiceDefinitionViewDTO(
    int Id,
    string Name,
    string Url,
    int ChecksCount
);
