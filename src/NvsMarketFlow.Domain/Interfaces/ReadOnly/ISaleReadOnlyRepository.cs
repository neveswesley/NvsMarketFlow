using NvsMarketFlow.Domain.Common;
using NvsMarketFlow.Domain.Entities;
using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Domain.Interfaces.ReadOnly;

public interface ISaleReadOnlyRepository
{
    Task<Sale?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<int> GetNextSaleNumberAsync(CancellationToken ct);
    Task<PagedResult<Sale>> GetAllAsync(
        Guid? cashRegisterId,
        Guid? sellerId,
        string? saleNumber,
        SaleStatus? status,
        DateTime? startDate,
        DateTime? endDate,
        int page,
        int pageSize,
        CancellationToken ct);
}