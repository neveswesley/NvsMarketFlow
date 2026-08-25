using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Application.Responses.StockMovement;

public class GetStockMovementResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public MovementType MovementType { get; set; }
    public decimal Quantity { get; set; }
    public string Reason { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}