using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Application.Responses.Purchase;

public class GetPurchaseResponse
{
    public Guid Id { get; set; }
    public Guid SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public string InvoiceNumber { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public PurchaseStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}