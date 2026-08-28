using Microsoft.EntityFrameworkCore;
using NvsMarketFlow.Domain.Common;
using NvsMarketFlow.Domain.Entities;
using NvsMarketFlow.Domain.Enums;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;
using NvsMarketFlow.Infrastructure.DataAccess;

namespace NvsMarketFlow.Infrastructure.Repositories;

public class SupplierRepository : ISupplierWriteOnlyRepository, ISupplierReadOnlyRepository
{
    private readonly AppDbContext _dbContext;

    public SupplierRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Supplier> CreateAsync(Supplier supplier, CancellationToken ct)
    {
        await _dbContext.Suppliers.AddAsync(supplier, ct);
        return supplier;
    }

    public Task UpdateAsync(Supplier supplier, CancellationToken ct)
    {
        _dbContext.Update(supplier);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsByCorporateNameAsync(string corporateName, CancellationToken ct)
    {
        return await _dbContext.Suppliers.AnyAsync(s => s.CorporateName == corporateName, ct);
    }

    public async Task<bool> ExistsByCnpjAsync(string cnpj, CancellationToken ct)
    {
        return await _dbContext.Suppliers.AnyAsync(s => s.CNPJ == cnpj, ct);
    }

    public async Task<Supplier?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbContext.Suppliers.FirstOrDefaultAsync(s => s.Id == id, ct);
    }
    
    public async Task<PagedResult<Supplier>> GetAllAsync(
        string? corporateName,
        string? fantasyName,
        string? cnpj,
        Status? status,
        int page,
        int pageSize,
        CancellationToken ct)
    {
        var query = _dbContext.Suppliers
            .AsNoTracking()
            .Where(s => s.Status != Status.Inactive)
            .AsQueryable();

        //corporateName
        if (!string.IsNullOrWhiteSpace(corporateName))
            query = query.Where(s => s.CorporateName.Contains(corporateName));

        //fantasyName
        if (!string.IsNullOrWhiteSpace(fantasyName))
            query = query.Where(s => s.FantasyName.Contains(fantasyName));

        //cnpj
        if (!string.IsNullOrWhiteSpace(cnpj))
            query = query.Where(s => s.CNPJ == cnpj);

        //status
        if (status.HasValue)
            query = query.Where(s => s.Status == status.Value);

        var totalItems = await query.CountAsync(ct);

        var items = await query
            .OrderBy(s => s.CorporateName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        return new PagedResult<Supplier>(
            items,
            page,
            pageSize,
            totalItems,
            totalPages);
    }
    
    public async Task<bool> ExistsByCorporateNameAsync(string corporateName, Guid? excludeId, CancellationToken ct)
    {
        return await _dbContext.Suppliers
            .AnyAsync(s => s.CorporateName == corporateName && (!excludeId.HasValue || s.Id != excludeId.Value), ct);
    }

    public async Task<bool> ExistsByCnpjAsync(string cnpj, Guid? excludeId, CancellationToken ct)
    {
        return await _dbContext.Suppliers
            .AnyAsync(s => s.CNPJ == cnpj && (!excludeId.HasValue || s.Id != excludeId.Value), ct);
    }
}