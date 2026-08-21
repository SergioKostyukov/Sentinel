using SentinelApi.Monitoring.Application.Models.ServiceDefinition;
using SentinelApi.Monitoring.Domain.Entities;

namespace SentinelApi.Monitoring.Application.Mappers;

internal static class ServiceDefinitionMapper
{
    internal static ServiceDefinition ToEntity(this CreateServiceDefinitionRequest request)
        => new ServiceDefinition(
            name: request.Name,
            url: request.Url,
            notificationEmails: request.NotificationEmails,
            description: request.Description
        );

    internal static ServiceDefinitionDTO ToDto(this ServiceDefinition serviceDefinition)
       => new ServiceDefinitionDTO(
            Id: serviceDefinition.Id,
            Name: serviceDefinition.Name,
            Url: serviceDefinition.Url,
            NotificationEmails: serviceDefinition.NotificationEmails,
            Description: serviceDefinition.Description,
            Checks: serviceDefinition.Checks?.ToDictionary(c => c.Id, c => c.Name) ?? []);

    internal static List<ServiceDefinitionViewDTO> ToViewDtoList(this IEnumerable<ServiceDefinition> serviceDefinitions)
        => [.. serviceDefinitions.Select(sd => new ServiceDefinitionViewDTO(
            Id: sd.Id,
            Name: sd.Name,
            Url: sd.Url,
            ChecksCount: sd.Checks?.Count ?? 0
        ))];
}
