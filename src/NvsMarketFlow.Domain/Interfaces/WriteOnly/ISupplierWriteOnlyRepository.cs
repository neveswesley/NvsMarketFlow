using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Domain.Interfaces.WriteOnly;

public interface ISupplierWriteOnlyRepository
{
    Task<Supplier> CreateAsync(Supplier supplier, CancellationToken ct);
    Task UpdateAsync(Supplier supplier, CancellationToken ct);
}