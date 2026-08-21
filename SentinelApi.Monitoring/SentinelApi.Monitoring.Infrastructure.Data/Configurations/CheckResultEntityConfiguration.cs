using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SentinelApi.Monitoring.Domain.Entities;

namespace SentinelApi.Monitoring.Infrastructure.Data.Configurations;

public class CheckResultEntityConfiguration : IEntityTypeConfiguration<CheckResult>
{
    public void Configure(EntityTypeBuilder<CheckResult> builder)
    {
        builder.ToTable("CheckResults");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CheckId).IsRequired();
        builder.Property(x => x.CheckedAt).IsRequired();
        builder.Property(x => x.TriggerType).IsRequired().HasConversion<int>();
        builder.Property(x => x.HealthStatus).IsRequired().HasConversion<int>();
        builder.Property(x => x.ResponseJson).IsRequired();
        builder.Property(x => x.ErrorMessage);
    }
}
