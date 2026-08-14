using NvsMarketFlow.Application.Common;
using NvsMarketFlow.Domain.Entities;
using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Domain.Interfaces.ReadOnly;

public interface IBrandReadOnlyRepository
{
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct);
    Task<Brand?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<PagedResult<Brand>> GetAllAsync(
        string? name,
        int page,
        int pageSize,
        CancellationToken ct);
}