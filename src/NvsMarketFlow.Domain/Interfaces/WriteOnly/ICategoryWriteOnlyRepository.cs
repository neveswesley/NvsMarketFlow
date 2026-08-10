using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Domain.Interfaces.WriteOnly;

public interface ICategoryWriteOnlyRepository
{
    Task<Category> CreateAsync(Category category);
    Task<Category> UpdateAsync(Category category);
}