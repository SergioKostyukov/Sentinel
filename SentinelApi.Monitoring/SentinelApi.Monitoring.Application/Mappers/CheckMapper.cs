using SentinelApi.Monitoring.Application.Helpers;
using SentinelApi.Monitoring.Application.Models.Check;
using SentinelApi.Monitoring.Domain.Entities;
using SentinelApi.Monitoring.Domain.Enums;

namespace SentinelApi.Monitoring.Application.Mappers;

internal static class CheckMapper
{
    internal static Check ToEntity(this CreateCheckRequest request)
        => new Check(
            serviceDefinitionId: request.ServiceDefinitionId,
            name: request.Name,
            endpointUrl: request.EndpointUrl,
            description: request.Description,
            probeType: (ProbeType)request.ProbeType,
            isEnabled: request.IsEnabled
        );

    internal static CheckDTO ToDto(this Check check)
       => new CheckDTO(
           Id: check.Id,
           ServiceDefinitionId: check.ServiceDefinitionId,
           Name: check.Name,
           EndpointUrl: check.EndpointUrl,
           Description: check.Description,
           ProbeType: (int)check.ProbeType,
           IsEnabled: check.IsEnabled
       );

    internal static List<CheckViewDTO> ToViewDtoList(this List<Check> checks)
        => [.. checks.Select(check => new CheckViewDTO(
            Id: check.Id,
            Name: check.Name,
            ServiceName: check.ServiceDefinition.Name,
            ProbeType: check.ProbeType.GetEnumDescription(),
            IsEnabled: check.IsEnabled
        ))];
}
