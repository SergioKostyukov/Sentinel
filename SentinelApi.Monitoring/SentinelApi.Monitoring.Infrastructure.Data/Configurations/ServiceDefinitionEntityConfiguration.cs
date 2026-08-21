using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SentinelApi.Monitoring.Domain.Entities;

namespace SentinelApi.Monitoring.Infrastructure.Data.Configurations;

public class ServiceDefinitionEntityConfiguration : IEntityTypeConfiguration<ServiceDefinition>
{
    public void Configure(EntityTypeBuilder<ServiceDefinition> builder)
    {
        builder.ToTable("ServiceDefinitions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Url).IsRequired();
        builder.Property(x => x.NotificationEmails).IsRequired();
        builder.Property(x => x.Description);
    }
}
