using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Infrastructure.Configuration;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.Property(a => a.Action)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.Entity)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.OldValue)
            .HasColumnType("nvarchar(max)");

        builder.Property(a => a.NewValue)
            .HasColumnType("nvarchar(max)");

        builder.HasIndex(a => a.UserId);
        builder.HasIndex(a => a.Entity);
        builder.HasIndex(a => a.Date);
    }
}