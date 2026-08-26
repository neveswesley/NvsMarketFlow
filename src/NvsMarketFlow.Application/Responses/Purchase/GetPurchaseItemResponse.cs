namespace NvsMarketFlow.Application.Responses.Purchase;

public class GetPurchaseItemResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal CostPrice { get; set; }
    public decimal Subtotal => Quantity * CostPrice;
}