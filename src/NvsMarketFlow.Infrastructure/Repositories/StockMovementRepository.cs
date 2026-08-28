using Microsoft.EntityFrameworkCore;
using NvsMarketFlow.Domain.Common;
using NvsMarketFlow.Domain.Entities;
using NvsMarketFlow.Domain.Enums;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;
using NvsMarketFlow.Infrastructure.DataAccess;

namespace NvsMarketFlow.Infrastructure.Repositories;

public class StockMovementRepository : IStockMovementReadOnlyRepository, IStockMovementWriteOnlyRepository
{
    
    private readonly AppDbContext _context;

    public StockMovementRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<StockMovement> CreateAsync(StockMovement stockMovement, CancellationToken ct)
    {
        await _context.StockMovements.AddAsync(stockMovement, ct);
        return stockMovement;
    }

    public async Task<PagedResult<StockMovement>> GetAllAsync(Guid? productId, Guid? userId, MovementType? movementType, DateTime? startDate, DateTime? endDate,
        int page, int pageSize, CancellationToken ct)
    {
        var query = _context.StockMovements
            .Include(sm => sm.Product)
            .Include(sm => sm.User)
            .AsNoTracking()
            .AsQueryable();

        //productId
        if (productId.HasValue)
            query = query.Where(sm => sm.ProductId == productId.Value);

        //userId
        if (userId.HasValue)
            query = query.Where(sm => sm.UserId == userId.Value);

        //movementType
        if (movementType.HasValue)
            query = query.Where(sm => sm.MovementType == movementType.Value);

        //startDate
        if (startDate.HasValue)
            query = query.Where(sm => sm.Date >= startDate.Value);

        //endDate
        if (endDate.HasValue)
            query = query.Where(sm => sm.Date <= endDate.Value);

        var totalItems = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(sm => sm.Date)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        return new PagedResult<StockMovement>(
            items,
            page,
            pageSize,
            totalItems,
            totalPages);
    }

    public async Task<StockMovement?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.StockMovements
            .Include(sm => sm.Product)
            .Include(sm => sm.User)
            .FirstOrDefaultAsync(sm => sm.Id == id, ct);
    }
}