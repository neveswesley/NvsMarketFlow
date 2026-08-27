namespace NvsMarketFlow.Application.Responses.Sale;

public class AddSaleItemResponse
{
    public Guid SaleId { get; set; }
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal ItemTotal { get; set; }
    public decimal SaleSubtotal { get; set; }
    public decimal SaleTotal { get; set; }
}