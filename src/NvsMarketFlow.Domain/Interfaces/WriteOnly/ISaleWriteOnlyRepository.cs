using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Domain.Interfaces.WriteOnly;

public interface ISaleWriteOnlyRepository
{
    Task<Sale> CreateAsync(Sale sale, CancellationToken ct);
    Task UpdateAsync(Sale sale, CancellationToken ct);
    Task AddItemAsync(SaleItem item, CancellationToken ct);
    Task AddPaymentAsync(Payment payment, CancellationToken ct);
}