using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Infrastructure.Configuration;

public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
{
    public void Configure(EntityTypeBuilder<Purchase> builder)
    {
        builder.Property(p => p.InvoiceNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Total)
            .HasPrecision(18, 2);

        builder.Property(p => p.Status)
            .IsRequired();

        builder.HasIndex(p => p.InvoiceNumber)
            .IsUnique();

        builder.HasOne(p => p.Supplier)
            .WithMany()
            .HasForeignKey(p => p.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}