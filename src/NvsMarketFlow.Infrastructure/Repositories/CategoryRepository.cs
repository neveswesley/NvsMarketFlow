using NvsMarketFlow.Domain.Entities;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;
using NvsMarketFlow.Infrastructure.DataAccess;

namespace NvsMarketFlow.Infrastructure.Repositories;

public class CategoryRepository : ICategoryWriteOnlyRepository
{

    private readonly AppDbContext _dbContext;

    public CategoryRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Category> CreateAsync(Category category)
    {
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();
        return category;
    }
}