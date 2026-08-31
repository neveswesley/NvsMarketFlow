using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Application.Responses.Product;

public class GetProductByBarcodeResponse
{
    public Guid Id { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string? BrandName { get; set; }
    public string? SupplierName { get; set; }
    public decimal SalePrice { get; set; }
    public decimal CurrentStock { get; set; }
    public decimal MinimumStock { get; set; }
    public Unit Unit { get; set; }
    public Status Status { get; set; }
}