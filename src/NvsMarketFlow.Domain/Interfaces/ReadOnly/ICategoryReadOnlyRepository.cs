using NvsMarketFlow.Domain.Common;
using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Domain.Interfaces.ReadOnly;

public interface ICategoryReadOnlyRepository
{
    Task<PagedResult<Category>> GetAllAsync(string? name, int page, int pageSize, CancellationToken ct);
    Task<Category?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct);
    Task<bool> HasLinkedProductsAsync(Guid categoryId, CancellationToken ct);

}