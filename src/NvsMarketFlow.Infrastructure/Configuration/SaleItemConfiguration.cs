using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Infrastructure.Configuration;

public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.Property(si => si.Quantity)
            .HasPrecision(18, 3);

        builder.Property(si => si.UnitPrice)
            .HasPrecision(18, 2);

        builder.Property(si => si.Discount)
            .HasPrecision(18, 2);

        builder.Property(si => si.Total)
            .HasPrecision(18, 2);

        builder.HasOne(si => si.Sale)
            .WithMany(s => s.Items)
            .HasForeignKey(si => si.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(si => si.Product)
            .WithMany()
            .HasForeignKey(si => si.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}