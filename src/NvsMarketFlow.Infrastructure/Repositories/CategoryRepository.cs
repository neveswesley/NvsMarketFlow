using Microsoft.EntityFrameworkCore;
using NvsMarketFlow.Domain.Common;
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
        return category;
    }

    public void UpdateAsync(Category category)
    {
        _dbContext.Update(category);
    }

    public void DeleteAsync(Category category)
    {
        _dbContext.Categories.Remove(category);
    }

    public async Task<PagedResult<Category>> GetAllAsync(
        string? name,
        int page,
        int pageSize,
        CancellationToken ct)
    {
        var query = _dbContext.Categories
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(c => c.Name.Contains(name));
        }

        var totalItems = await query.CountAsync(ct);

        var items = await query
            .OrderBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var totalPages = (int)Math.Ceiling(
            (double)totalItems / pageSize);

        return new PagedResult<Category>(
            items,
            page,
            pageSize,
            totalItems,
            totalPages);
    }

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbContext.Categories.Include(c=>c.Products).FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken ct)
    {
        return await _dbContext.Categories
            .AnyAsync(c => c.Name == name, ct);
    }
    
    public async Task<bool> HasLinkedProductsAsync(Guid categoryId, CancellationToken ct)
    {
        return await _dbContext.Products.AnyAsync(p => p.CategoryId == categoryId, ct);
    }
}