using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Application.Responses.Sale;

public class GetSaleByIdResponse
{
    public Guid Id { get; set; }
    public Guid CashRegisterId { get; set; }
    public Guid SellerId { get; set; }
    public string SellerName { get; set; } = string.Empty;
    public string SaleNumber { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public decimal Discount { get; set; }
    public decimal Total { get; set; }
    public SaleStatus Status { get; set; }
    public List<GetSaleItemResponse> Items { get; set; } = new();
    public List<GetPaymentResponse> Payments { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}