using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Domain.Interfaces.WriteOnly;

public interface ICashMovementWriteOnlyRepository
{
    Task<CashMovement> CreateAsync(CashMovement cashMovement, CancellationToken ct);
}