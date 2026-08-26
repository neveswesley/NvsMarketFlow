namespace NvsMarketFlow.Application.Requests.Purchase;

public class AddPurchaseItemRequest
{
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; }
    public decimal CostPrice { get; set; }
}