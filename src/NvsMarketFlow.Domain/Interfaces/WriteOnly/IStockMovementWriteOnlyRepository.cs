using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Domain.Interfaces.WriteOnly;

public interface IStockMovementWriteOnlyRepository
{
    Task<StockMovement> CreateAsync(StockMovement stockMovement);
}