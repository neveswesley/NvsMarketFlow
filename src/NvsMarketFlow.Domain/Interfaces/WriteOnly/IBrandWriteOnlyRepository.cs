using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Domain.Interfaces.WriteOnly;

public interface IBrandWriteOnlyRepository
{
    Task<Brand> CreateAsync(Brand brand, CancellationToken ct);
    Task DeleteAsync(Brand brand, CancellationToken ct);
}