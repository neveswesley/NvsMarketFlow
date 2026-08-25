using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Application.Requests.Product;

public class UpdateProductInfoRequest
{
    public string Sku { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public Guid CategoryId { get; set; }
    public Guid? BrandId { get; set; }
    public Guid? SupplierId { get; set; }

    public decimal CostPrice { get; set; }
    public decimal SalePrice { get; set; }

    public decimal MinimumStock { get; set; }
    public decimal MaximumStock { get; set; }

    public Unit Unit { get; set; }
}