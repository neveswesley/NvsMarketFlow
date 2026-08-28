using Microsoft.EntityFrameworkCore;
using NvsMarketFlow.Domain.Common;
using NvsMarketFlow.Domain.Entities;
using NvsMarketFlow.Domain.Enums;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;
using NvsMarketFlow.Infrastructure.DataAccess;

namespace NvsMarketFlow.Infrastructure.Repositories;

public class SaleRepository : ISaleWriteOnlyRepository, ISaleReadOnlyRepository
{
    private readonly AppDbContext _dbContext;

    public SaleRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Sale> CreateAsync(Sale sale, CancellationToken ct)
    {
        await _dbContext.Sales.AddAsync(sale, ct);
        return sale;
    }

    public Task UpdateAsync(Sale sale, CancellationToken ct)
    {
        _dbContext.Update(sale);
        return Task.CompletedTask;
    }

    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbContext.Sales
            .Include(s => s.CashRegister)
            .Include(s => s.Seller)
            .Include(s => s.Items)
            .ThenInclude(i => i.Product)
            .Include(s => s.Payments)
            .FirstOrDefaultAsync(s => s.Id == id, ct);
    }
    
    public async Task<int> GetNextSaleNumberAsync(CancellationToken ct)
    {
        var result = await _dbContext.Database
            .SqlQueryRaw<int>("SELECT NEXT VALUE FOR SaleNumberSequence")
            .ToListAsync(ct);

        return result.First();
    }

    public async Task<PagedResult<Sale>> GetAllAsync(
        Guid? cashRegisterId,
        Guid? sellerId,
        string? saleNumber,
        SaleStatus? status,
        DateTime? startDate,
        DateTime? endDate,
        int page,
        int pageSize,
        CancellationToken ct)
    {
        var query = _dbContext.Sales
            .Include(s => s.Seller)
            .AsNoTracking()
            .AsQueryable();

        //cashRegisterId
        if (cashRegisterId.HasValue)
            query = query.Where(s => s.CashRegisterId == cashRegisterId.Value);

        //sellerId
        if (sellerId.HasValue)
            query = query.Where(s => s.SellerId == sellerId.Value);

        //saleNumber
        if (!string.IsNullOrWhiteSpace(saleNumber))
            query = query.Where(s => s.SaleNumber.Contains(saleNumber));

        //status
        if (status.HasValue)
            query = query.Where(s => s.Status == status.Value);

        //startDate
        if (startDate.HasValue)
            query = query.Where(s => s.CreatedAt >= startDate.Value);

        //endDate
        if (endDate.HasValue)
            query = query.Where(s => s.CreatedAt <= endDate.Value);

        var totalItems = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(s => s.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        return new PagedResult<Sale>(
            items,
            page,
            pageSize,
            totalItems,
            totalPages);
    }

    public async Task AddItemAsync(SaleItem item, CancellationToken ct)
    {
        await _dbContext.SaleItems.AddAsync(item, ct);
    }
    
    public async Task AddPaymentAsync(Payment payment, CancellationToken ct)
    {
        await _dbContext.Payments.AddAsync(payment, ct);
    }
}