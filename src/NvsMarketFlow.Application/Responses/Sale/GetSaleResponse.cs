using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Application.Responses.Sale;

public class GetSaleResponse
{
    public Guid Id { get; set; }
    public Guid SellerId { get; set; }
    public string SellerName { get; set; } = string.Empty;
    public string SaleNumber { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public SaleStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}