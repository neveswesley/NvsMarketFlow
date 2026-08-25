using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Infrastructure.Configuration;

public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> builder)
    {
        builder.Property(sm => sm.Quantity)
            .HasPrecision(18, 3);

        builder.Property(sm => sm.Reason)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(sm => sm.MovementType)
            .IsRequired();

        builder.Property(sm => sm.Date)
            .IsRequired();

        builder.HasOne(sm => sm.Product)
            .WithMany()
            .HasForeignKey(sm => sm.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sm => sm.User)
            .WithMany()
            .HasForeignKey(sm => sm.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(sm => sm.ProductId);
        builder.HasIndex(sm => sm.UserId);
        builder.HasIndex(sm => sm.Date);
    }
}