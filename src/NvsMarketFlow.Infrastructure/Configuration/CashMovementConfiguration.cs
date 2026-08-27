using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Infrastructure.Configuration;

public class CashMovementConfiguration : IEntityTypeConfiguration<CashMovement>
{
    public void Configure(EntityTypeBuilder<CashMovement> builder)
    {
        builder.Property(cm => cm.Value)
            .HasPrecision(18, 2);

        builder.Property(cm => cm.Reason)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(cm => cm.Type)
            .IsRequired();

        builder.HasOne(cm => cm.CashRegister)
            .WithMany()
            .HasForeignKey(cm => cm.CashRegisterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(cm => cm.CashRegisterId);
        builder.HasIndex(cm => cm.Type);
    }
}