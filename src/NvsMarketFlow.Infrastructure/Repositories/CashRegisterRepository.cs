using Microsoft.EntityFrameworkCore;
using NvsMarketFlow.Domain.Common;
using NvsMarketFlow.Domain.Entities;
using NvsMarketFlow.Domain.Enums;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;
using NvsMarketFlow.Infrastructure.DataAccess;

namespace NvsMarketFlow.Infrastructure.Repositories;

public class CashRegisterRepository : ICashRegisterWriteOnlyRepository, ICashRegisterReadOnlyRepository
{
    private readonly AppDbContext _dbContext;

    public CashRegisterRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CashRegister> CreateAsync(CashRegister cashRegister, CancellationToken ct)
    {
        await _dbContext.CashRegisters.AddAsync(cashRegister, ct);
        return cashRegister;
    }

    public Task UpdateAsync(CashRegister cashRegister, CancellationToken ct)
    {
        _dbContext.Update(cashRegister);
        return Task.CompletedTask;
    }

    public async Task<bool> HasOpenCashRegisterAsync(Guid userId, CancellationToken ct)
    {
        return await _dbContext.CashRegisters
            .AnyAsync(c => c.UserId == userId && c.Status == CashRegisterStatus.Open, ct);
    }

    public async Task<CashRegister?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbContext.CashRegisters
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == id, ct);
    }
    
    public async Task<PagedResult<CashRegister>> GetAllAsync(
        Guid? userId,
        CashRegisterStatus? status,
        DateTime? startDate,
        DateTime? endDate,
        int page,
        int pageSize,
        CancellationToken ct)
    {
        var query = _dbContext.CashRegisters
            .Include(c => c.User)
            .AsNoTracking()
            .AsQueryable();

        //userId
        if (userId.HasValue)
            query = query.Where(c => c.UserId == userId.Value);

        //status
        if (status.HasValue)
            query = query.Where(c => c.Status == status.Value);

        //startDate
        if (startDate.HasValue)
            query = query.Where(c => c.OpenedAt >= startDate.Value);

        //endDate
        if (endDate.HasValue)
            query = query.Where(c => c.OpenedAt <= endDate.Value);

        var totalItems = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(c => c.OpenedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        return new PagedResult<CashRegister>(
            items,
            page,
            pageSize,
            totalItems,
            totalPages);
    }
}