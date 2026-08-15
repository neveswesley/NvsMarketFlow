using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Infrastructure.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.CostPrice)
            .HasPrecision(18, 2);

        builder.Property(p => p.SalePrice)
            .HasPrecision(18, 2);

        builder.Property(p => p.CurrentStock)
            .HasPrecision(18, 3);

        builder.Property(p => p.MinimumStock)
            .HasPrecision(18, 3);

        builder.Property(p => p.MaximumStock)
            .HasPrecision(18, 3);
    }
}