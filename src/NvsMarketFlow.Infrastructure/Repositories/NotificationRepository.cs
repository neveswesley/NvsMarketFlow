using Microsoft.EntityFrameworkCore;
using NvsMarketFlow.Domain.Common;
using NvsMarketFlow.Domain.Entities;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;
using NvsMarketFlow.Infrastructure.DataAccess;

namespace NvsMarketFlow.Infrastructure.Repositories;

public class NotificationRepository : INotificationWriteOnlyRepository, INotificationReadOnlyRepository
{
    private readonly AppDbContext _dbContext;

    public NotificationRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Notification> CreateAsync(Notification notification, CancellationToken ct)
    {
        await _dbContext.Notifications.AddAsync(notification, ct);
        return notification;
    }

    public async Task<Notification?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbContext.Notifications
            .FirstOrDefaultAsync(n => n.Id == id, ct);
    }

    public async Task<PagedResult<Notification>> GetAllAsync(
        Guid userId,
        bool? read,
        int page,
        int pageSize,
        CancellationToken ct)
    {
        var query = _dbContext.Notifications
            .AsNoTracking()
            .Where(n => n.UserId == userId)
            .AsQueryable();

        //read
        if (read.HasValue)
            query = query.Where(n => n.Read == read.Value);

        var totalItems = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        return new PagedResult<Notification>(items, page, pageSize, totalItems, totalPages);
    }
}