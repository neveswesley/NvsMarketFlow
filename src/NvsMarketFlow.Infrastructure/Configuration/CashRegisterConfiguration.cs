using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Infrastructure.Configuration;

public class CashRegisterConfiguration : IEntityTypeConfiguration<CashRegister>
{
    public void Configure(EntityTypeBuilder<CashRegister> builder)
    {
        builder.Property(c => c.OpeningBalance)
            .HasPrecision(18, 2);

        builder.Property(c => c.ClosingBalance)
            .HasPrecision(18, 2);

        builder.Property(c => c.Status)
            .IsRequired();

        builder.HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.UserId);
        builder.HasIndex(c => c.Status);
    }
}