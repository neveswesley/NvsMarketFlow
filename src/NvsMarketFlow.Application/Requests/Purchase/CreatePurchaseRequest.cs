namespace NvsMarketFlow.Application.Requests.Purchase;

public class CreatePurchaseRequest
{
    public Guid SupplierId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
}