using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Domain.Interfaces.WriteOnly;

public interface IProductWriteOnlyRepository
{
    Task<Product> CreateAsync(Product product);
}