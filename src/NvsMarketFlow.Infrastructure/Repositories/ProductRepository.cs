using NvsMarketFlow.Domain.Entities;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;
using NvsMarketFlow.Infrastructure.DataAccess;

namespace NvsMarketFlow.Infrastructure.Repositories;

public class ProductRepository : IProductWriteOnlyRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<Product> CreateAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        return product;
    }
}