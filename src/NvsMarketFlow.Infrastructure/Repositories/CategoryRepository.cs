using Microsoft.EntityFrameworkCore;
using NvsMarketFlow.Domain.Entities;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;
using NvsMarketFlow.Infrastructure.DataAccess;

namespace NvsMarketFlow.Infrastructure.Repositories;

public class CategoryRepository : ICategoryWriteOnlyRepository, ICategoryReadOnlyRepository
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

    public async Task<List<Category>> GetAll()
    {
        return await _dbContext.Categories.ToListAsync();
    }
}