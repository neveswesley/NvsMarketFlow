using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Application.Responses.Purchase;

public class CreatePurchaseResponse
{
    public Guid Id { get; set; }
    public Guid SupplierId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public PurchaseStatus Status { get; set; }
}