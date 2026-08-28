using Microsoft.EntityFrameworkCore;
using NvsMarketFlow.Domain.Common;
using NvsMarketFlow.Domain.Entities;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Infrastructure.DataAccess;

namespace NvsMarketFlow.Infrastructure.Repositories;

public class AuditLogRepository : IAuditLogReadOnlyRepository
{
    private readonly AppDbContext _dbContext;

    public AuditLogRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<AuditLog>> GetAllAsync(
        Guid? userId, string? entity, string? action,
        DateTime? startDate, DateTime? endDate,
        int page, int pageSize, CancellationToken ct)
    {
        var query = _dbContext.AuditLogs.AsNoTracking().AsQueryable();

        if (userId.HasValue) query = query.Where(a => a.UserId == userId.Value);
        if (!string.IsNullOrWhiteSpace(entity)) query = query.Where(a => a.Entity == entity);
        if (!string.IsNullOrWhiteSpace(action)) query = query.Where(a => a.Action == action);
        if (startDate.HasValue) query = query.Where(a => a.Date >= startDate.Value);
        if (endDate.HasValue) query = query.Where(a => a.Date <= endDate.Value);

        var totalItems = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(a => a.Date)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        return new PagedResult<AuditLog>(items, page, pageSize, totalItems, totalPages);
    }
}