using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Application.Requests.StockMovement;

public class CreateStockMovementRequest
{
    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }
    public MovementType MovementType { get; set; }
    public decimal Quantity { get; set; }
    public string Reason { get; set; }
    public bool? IsIncrease { get; set; }
}