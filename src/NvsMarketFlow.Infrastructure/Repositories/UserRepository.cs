using Microsoft.EntityFrameworkCore;
using NvsMarketFlow.Application.Common;
using NvsMarketFlow.Domain.Entities;
using NvsMarketFlow.Domain.Enums;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;
using NvsMarketFlow.Infrastructure.DataAccess;

namespace NvsMarketFlow.Infrastructure.Repositories;

public class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task<bool> EmailExists(string email, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.AnyAsync(e => e.Email == email, cancellationToken);
    }

    public async Task<User?> GetByEmail(string email, CancellationToken cancellationToken)
    {
        return  await _dbContext.Users.SingleOrDefaultAsync(e => e.Email == email, cancellationToken);
    }

    public async Task<User?> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<PagedResult<User>> GetAllAsync(string? name, int page, int pageSize, CancellationToken ct)
    {
        var query = _dbContext.Users.AsNoTracking().Where(u=>u.Status == UserStatus.Active).AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(b => b.Name.Contains(name));

        var totalItems = await query.CountAsync(ct);
        
        var items = await query
            .OrderBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var totalPages = (int)Math.Ceiling(
            (double)totalItems / pageSize);

        return new PagedResult<User>(
            items,
            page,
            pageSize,
            totalItems,
            totalPages);
        
    }
}