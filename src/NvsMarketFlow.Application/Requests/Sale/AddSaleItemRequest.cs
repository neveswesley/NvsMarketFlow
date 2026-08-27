namespace NvsMarketFlow.Application.Requests.Sale;

public class AddSaleItemRequest
{
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; }
    public decimal Discount { get; set; } = 0;
}