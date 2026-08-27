using Microsoft.EntityFrameworkCore;
using NvsMarketFlow.Application.Common;
using NvsMarketFlow.Domain.Entities;
using NvsMarketFlow.Domain.Enums;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;
using NvsMarketFlow.Infrastructure.DataAccess;

namespace NvsMarketFlow.Infrastructure.Repositories;

public class CashMovementRepository : ICashMovementWriteOnlyRepository, ICashMovementReadOnlyRepository
{
    private readonly AppDbContext _dbContext;

    public CashMovementRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CashMovement> CreateAsync(CashMovement cashMovement, CancellationToken ct)
    {
        await _dbContext.CashMovements.AddAsync(cashMovement, ct);
        return cashMovement;
    }

    public async Task<PagedResult<CashMovement>> GetAllAsync(
        Guid cashRegisterId,
        CashMovementType? type,
        int page,
        int pageSize,
        CancellationToken ct)
    {
        var query = _dbContext.CashMovements
            .AsNoTracking()
            .Where(cm => cm.CashRegisterId == cashRegisterId)
            .AsQueryable();

        //type
        if (type.HasValue)
            query = query.Where(cm => cm.Type == type.Value);

        var totalItems = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(cm => cm.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        return new PagedResult<CashMovement>(
            items,
            page,
            pageSize,
            totalItems,
            totalPages);
    }
    
    public async Task<List<CashMovement>> GetAllByCashRegisterIdAsync(Guid cashRegisterId, CancellationToken ct)
    {
        return await _dbContext.CashMovements
            .AsNoTracking()
            .Where(cm => cm.CashRegisterId == cashRegisterId)
            .ToListAsync(ct);
    }
}