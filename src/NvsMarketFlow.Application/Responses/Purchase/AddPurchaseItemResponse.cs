namespace NvsMarketFlow.Application.Responses.Purchase;

public class AddPurchaseItemResponse
{
    public Guid PurchaseId { get; set; }
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; }
    public decimal CostPrice { get; set; }
    public decimal PurchaseTotal { get; set; }
}