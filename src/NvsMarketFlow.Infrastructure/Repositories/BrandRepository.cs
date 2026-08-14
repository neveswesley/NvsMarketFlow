using Microsoft.EntityFrameworkCore;
using NvsMarketFlow.Application.Common;
using NvsMarketFlow.Domain.Entities;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;
using NvsMarketFlow.Infrastructure.DataAccess;

namespace NvsMarketFlow.Infrastructure.Repositories;

public class BrandRepository : IBrandWriteOnlyRepository, IBrandReadOnlyRepository
{
    
    private readonly AppDbContext _dbContext;

    public BrandRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Brand> CreateAsync(Brand brand, CancellationToken ct)
    {
        await _dbContext.AddAsync(brand, ct);
        await _dbContext.SaveChangesAsync(ct);
        return brand;
    }

    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Brand brand, CancellationToken ct)
    {
        _dbContext.Remove(brand);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken ct)
    {
        return await _dbContext.Brands.AnyAsync(b=>b.Name == name, ct);
    }

    public async Task<Brand?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbContext.Brands.FirstOrDefaultAsync(b => b.Id == id, ct);
;    }

    public async Task<PagedResult<Brand>> GetAllAsync(string? name, int page, int pageSize, CancellationToken ct)
    {
        var query = _dbContext.Brands.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(b => b.Name.Contains(name));

        var totalItems = await query.CountAsync(ct);
        
        var items = await query
            .OrderBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var totalPages = (int)Math.Ceiling(
            (double)totalItems / pageSize);

        return new PagedResult<Brand>(
            items,
            page,
            pageSize,
            totalItems,
            totalPages);
        
    }
}