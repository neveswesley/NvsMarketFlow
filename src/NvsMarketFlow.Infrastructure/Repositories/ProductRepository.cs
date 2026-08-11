using Microsoft.EntityFrameworkCore;
using NvsMarketFlow.Domain.Entities;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;
using NvsMarketFlow.Infrastructure.DataAccess;

namespace NvsMarketFlow.Infrastructure.Repositories;

public class ProductRepository : IProductWriteOnlyRepository, IProductReadOnlyRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<Product> CreateAsync(Product product, CancellationToken ct)
    {
        await _context.Products.AddAsync(product, ct);
        await _context.SaveChangesAsync(ct);
        return product;
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken ct)
    {
        return await _context.Products.AnyAsync(p => p.Name == name, ct);
    }

    public async Task<bool> ExistsBySkuAsync(string sku, CancellationToken ct)
    {
        return await _context.Products.AnyAsync(p => p.Sku == sku, ct);
    }

    public async Task<bool> ExistsByBarcodeAsync(string barcode, CancellationToken ct)
    {
        return await _context.Products.AnyAsync(p => p.Barcode == barcode, ct);
    }
}