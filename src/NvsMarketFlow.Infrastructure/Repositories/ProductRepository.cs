using Microsoft.EntityFrameworkCore;
using NvsMarketFlow.Domain.Common;
using NvsMarketFlow.Domain.Entities;
using NvsMarketFlow.Domain.Enums;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;
using NvsMarketFlow.Infrastructure.DataAccess;

namespace NvsMarketFlow.Infrastructure.Repositories;

public class ProductRepository : IProductWriteOnlyRepository, IProductReadOnlyRepository
{
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<Product> CreateAsync(Product product, CancellationToken ct)
    {
        await _dbContext.Products.AddAsync(product, ct);
        return product;
    }

    public Task UpdateAsync(Product product, CancellationToken ct)
    {
        _dbContext.Update(product);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsByNameAsync(string name, Guid? excludeId, CancellationToken ct)
    {
        return await _dbContext.Products
            .AnyAsync(p => p.Name == name && (!excludeId.HasValue || p.Id != excludeId.Value), ct);
    }

    public async Task<bool> ExistsBySkuAsync(string sku, Guid? excludeId, CancellationToken ct)
    {
        return await _dbContext.Products
            .AnyAsync(p => p.Sku == sku && (!excludeId.HasValue || p.Id != excludeId.Value), ct);
    }

    public async Task<bool> ExistsByBarcodeAsync(string barcode, Guid? excludeId, CancellationToken ct)
    {
        return await _dbContext.Products
            .AnyAsync(p => p.Barcode == barcode && (!excludeId.HasValue || p.Id != excludeId.Value), ct);
    }

    public async Task<PagedResult<Product>> GetAllAsync(
        string? name,
        Guid? categoryId, 
        Guid? brandId,
        Status? status,
        decimal? minPrice,
        decimal? maxPrice,
        bool? lowStock,
        int page,
        int pageSize,
        CancellationToken ct)
    {
        var query = _dbContext.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .AsNoTracking()
            .Where(p => p.Status != Status.Inactive)
            .AsQueryable();

        //name
        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(c => c.Name.Contains(name));
        
        //categoryId
        if (categoryId.HasValue)
            query = query.Where(p=>p.CategoryId == categoryId.Value);
        
        //brandId
        if (brandId.HasValue)
            query = query.Where(p => p.BrandId == brandId.Value);
        
        //status
        if (status.HasValue)
            query = query.Where(p => p.Status == status.Value);
        
        //min price
        if (minPrice.HasValue)
            query = query.Where(p=> p.SalePrice >= minPrice.Value);
        
        //max price
        if (maxPrice.HasValue)
            query = query.Where(p => p.SalePrice <= maxPrice.Value);
        
        //low stock
        if (lowStock == true)
            query = query.Where(p => p.CurrentStock <= p.MinimumStock);

        var totalItems = await query.CountAsync(ct);

        var items = await query
            .OrderBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var totalPages = (int)Math.Ceiling(
            (double)totalItems / pageSize);

        return new PagedResult<Product>(
            items,
            page,
            pageSize,
            totalItems,
            totalPages);
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbContext.Products.Include(p => p.Category).Include(p => p.Brand)
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }
    
}