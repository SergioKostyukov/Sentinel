using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SentinelApi.Identity.Domain.Entities;

namespace SentinelApi.Identity.Infrastructure.Data.Configurations;

public sealed class ActionLogConfiguration : IEntityTypeConfiguration<ActionLog>
{
    public void Configure(EntityTypeBuilder<ActionLog> builder)
    {
        builder.ToTable("ActionLogs");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.AuthorId).IsRequired();
        builder.Property(u => u.TargetId).IsRequired();
        builder.Property(u => u.ActionType).HasConversion<int>().IsRequired();
        builder.Property(u => u.DateTime).IsRequired();
        builder.Property(u => u.Description).IsRequired();

        builder.HasIndex(u => u.AuthorId);
        builder.HasIndex(u => u.TargetId);

        // Restrict, а не Cascade — записи журналу дій є аудиторським слідом і мають лишатись
        // навіть якщо користувача-автора буде видалено.
        builder.HasOne(al => al.Author)
               .WithMany(u => u.ActionLogsAsAuthor)
               .HasForeignKey(al => al.AuthorId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
