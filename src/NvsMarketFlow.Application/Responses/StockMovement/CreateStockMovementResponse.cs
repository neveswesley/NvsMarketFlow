using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Application.Responses.StockMovement;

public class CreateStockMovementResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }
    public MovementType MovementType { get; set; }
    public decimal Quantity { get; set; }
    public string Reason { get; set; }
}