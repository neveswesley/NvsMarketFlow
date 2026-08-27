using NvsMarketFlow.Application.Common;
using NvsMarketFlow.Domain.Entities;
using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Domain.Interfaces.ReadOnly;

public interface ICashMovementReadOnlyRepository
{
    Task<PagedResult<CashMovement>> GetAllAsync(
        Guid cashRegisterId,
        CashMovementType? type,
        int page,
        int pageSize,
        CancellationToken ct);
    
    Task<List<Domain.Entities.CashMovement>> GetAllByCashRegisterIdAsync(Guid cashRegisterId, CancellationToken ct);
}