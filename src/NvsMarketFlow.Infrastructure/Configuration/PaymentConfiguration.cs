using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Infrastructure.Configuration;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.Property(p => p.Value)
            .HasPrecision(18, 2);

        builder.Property(p => p.Method)
            .IsRequired();

        builder.HasOne(p => p.Sale)
            .WithMany(s => s.Payments)
            .HasForeignKey(p => p.SaleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}