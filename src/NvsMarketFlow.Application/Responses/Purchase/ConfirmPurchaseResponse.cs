using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Application.Responses.Purchase;

public class ConfirmPurchaseResponse
{
    public Guid Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public PurchaseStatus Status { get; set; }
    public List<string> Warnings { get; set; } = new();
}