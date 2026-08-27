using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Infrastructure.Configuration;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.Property(s => s.SaleNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.Subtotal)
            .HasPrecision(18, 2);

        builder.Property(s => s.Discount)
            .HasPrecision(18, 2);

        builder.Property(s => s.Total)
            .HasPrecision(18, 2);

        builder.Property(s => s.Status)
            .IsRequired();

        builder.HasIndex(s => s.SaleNumber)
            .IsUnique();

        builder.HasOne(s => s.CashRegister)
            .WithMany()
            .HasForeignKey(s => s.CashRegisterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Seller)
            .WithMany()
            .HasForeignKey(s => s.SellerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}