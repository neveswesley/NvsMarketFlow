using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Domain.Interfaces.ReadOnly;

public interface ICategoryReadOnlyRepository
{
    Task<List<Category>> GetAll();
}