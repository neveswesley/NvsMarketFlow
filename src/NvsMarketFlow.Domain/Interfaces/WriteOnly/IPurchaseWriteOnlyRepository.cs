using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Domain.Interfaces.WriteOnly;

public interface IPurchaseWriteOnlyRepository
{
    Task<Purchase> CreateAsync(Purchase purchase, CancellationToken ct);
    Task AddItemAsync(PurchaseItem item, CancellationToken ct);
    Task UpdateAsync(Purchase purchase, CancellationToken ct);
}