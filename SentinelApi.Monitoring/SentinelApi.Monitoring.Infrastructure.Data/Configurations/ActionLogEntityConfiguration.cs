using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SentinelApi.Monitoring.Domain.Entities;

namespace SentinelApi.Monitoring.Infrastructure.Data.Configurations;

public class ActionLogEntityConfiguration : IEntityTypeConfiguration<ActionLog>
{
    public void Configure(EntityTypeBuilder<ActionLog> builder)
    {
        builder.ToTable("ActionLogs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.UserLogin).IsRequired();
        builder.Property(x => x.TargetId).IsRequired();
        builder.Property(x => x.TargetName).IsRequired();
        builder.Property(x => x.ActionType).IsRequired().HasConversion<int>();
        builder.Property(x => x.DateTime).IsRequired();
        builder.Property(x => x.Description);
    }
}
