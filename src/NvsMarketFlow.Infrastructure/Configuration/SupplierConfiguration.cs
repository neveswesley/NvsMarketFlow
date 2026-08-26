using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Infrastructure.Configuration;

public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.Property(s => s.CorporateName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.FantasyName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(s => s.CNPJ)
            .IsRequired()
            .HasMaxLength(18);

        builder.Property(s => s.Phone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(s => s.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(s => s.Address)
            .IsRequired()
            .HasMaxLength(250);

        builder.HasIndex(s => s.CNPJ)
            .IsUnique();

        builder.HasMany(s => s.Products)
            .WithOne(p => p.Supplier)
            .HasForeignKey(p => p.SupplierId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasMany<Purchase>()
            .WithOne(p => p.Supplier)
            .HasForeignKey(p => p.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}