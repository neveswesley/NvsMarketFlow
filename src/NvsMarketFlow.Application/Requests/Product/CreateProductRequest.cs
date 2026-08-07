using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Application.Requests.Product;

public class CreateProductRequest
{
    public string Sku { get; set; } = string.Empty;
    
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public Guid CategoryId { get; set; }
    
    public Guid BrandId { get; set; }
    
    public decimal CostPrice { get; set; }
    public decimal SalePrice { get; set; }
    public decimal CurrentStock { get; set; }
    public decimal MinimumStock { get; set; }
    public decimal MaximumStock { get; set; }
    
    public DateTime? ExpirationDate { get; set; }
    
    public Guid UnitId { get; set; }
    
    public Status Status { get; set; }
}