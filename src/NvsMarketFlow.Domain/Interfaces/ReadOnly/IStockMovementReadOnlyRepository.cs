using NvsMarketFlow.Domain.Common;
using NvsMarketFlow.Domain.Entities;
using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Domain.Interfaces.ReadOnly;

public interface IStockMovementReadOnlyRepository
{
    Task<PagedResult<StockMovement>> GetAllAsync(
        Guid? productId,
        Guid? userId,
        MovementType? movementType,
        DateTime? startDate,
        DateTime? endDate,
        int page,
        int pageSize,
        CancellationToken ct);

    Task<StockMovement?> GetByIdAsync(Guid id, CancellationToken ct);
}