using Microsoft.EntityFrameworkCore;
using NvsMarketFlow.Domain.Common;
using NvsMarketFlow.Domain.Entities;
using NvsMarketFlow.Domain.Enums;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;
using NvsMarketFlow.Infrastructure.DataAccess;

namespace NvsMarketFlow.Infrastructure.Repositories;

public class PurchaseRepository : IPurchaseWriteOnlyRepository, IPurchaseReadOnlyRepository
{
    private readonly AppDbContext _dbContext;

    public PurchaseRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Purchase> CreateAsync(Purchase purchase, CancellationToken ct)
    {
        await _dbContext.Purchases.AddAsync(purchase, ct);
        return purchase;
    }

    public Task UpdateAsync(Purchase purchase, CancellationToken ct)
    {
        _dbContext.Update(purchase);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsByInvoiceNumberAsync(string invoiceNumber, Guid? excludeId, CancellationToken ct)
    {
        return await _dbContext.Purchases
            .AnyAsync(p => p.InvoiceNumber == invoiceNumber && (!excludeId.HasValue || p.Id != excludeId.Value), ct);
    }

    public async Task<Purchase?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbContext.Purchases
            .Include(p => p.Supplier)
            .Include(p => p.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }
    
    public async Task AddItemAsync(PurchaseItem item, CancellationToken ct)
    {
        await _dbContext.PurchaseItems.AddAsync(item, ct);
    }
    
    public async Task<PagedResult<Purchase>> GetAllAsync(
        Guid? supplierId,
        string? invoiceNumber,
        PurchaseStatus? status,
        DateTime? startDate,
        DateTime? endDate,
        int page,
        int pageSize,
        CancellationToken ct)
    {
        var query = _dbContext.Purchases
            .Include(p => p.Supplier)
            .AsNoTracking()
            .AsQueryable();

        //supplierId
        if (supplierId.HasValue)
            query = query.Where(p => p.SupplierId == supplierId.Value);

        //invoiceNumber
        if (!string.IsNullOrWhiteSpace(invoiceNumber))
            query = query.Where(p => p.InvoiceNumber.Contains(invoiceNumber));

        //status
        if (status.HasValue)
            query = query.Where(p => p.Status == status.Value);

        //startDate
        if (startDate.HasValue)
            query = query.Where(p => p.CreatedAt >= startDate.Value);

        //endDate
        if (endDate.HasValue)
            query = query.Where(p => p.CreatedAt <= endDate.Value);

        var totalItems = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        return new PagedResult<Purchase>(
            items,
            page,
            pageSize,
            totalItems,
            totalPages);
    }
}