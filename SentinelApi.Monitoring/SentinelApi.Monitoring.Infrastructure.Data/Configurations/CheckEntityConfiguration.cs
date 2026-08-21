using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SentinelApi.Monitoring.Domain.Entities;

namespace SentinelApi.Monitoring.Infrastructure.Data.Configurations;

public class CheckEntityConfiguration : IEntityTypeConfiguration<Check>
{
    public void Configure(EntityTypeBuilder<Check> builder)
    {
        builder.ToTable("Checks");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ServiceDefinitionId).IsRequired();
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.EndpointUrl).IsRequired();
        builder.Property(x => x.Description).IsRequired();
        builder.Property(x => x.ProbeType).IsRequired().HasConversion<int>();
        builder.Property(x => x.IsEnabled).IsRequired();
    }
}
