using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Domain.Interfaces.WriteOnly;

public interface ICategoryWriteOnlyRepository
{
    Task<Category> CreateAsync(Category category);
    void UpdateAsync(Category category);
    void DeleteAsync(Category category);
    
}