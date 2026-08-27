using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Application.Responses.Sale;

public class GetSaleItemResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal Total { get; set; }
}