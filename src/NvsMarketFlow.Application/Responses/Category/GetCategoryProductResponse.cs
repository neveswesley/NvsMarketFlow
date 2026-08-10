namespace NvsMarketFlow.Application.Responses.Category;

public class GetCategoryProductResponse
{
    public string Sku { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;
    
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public decimal CostPrice { get; set; }
    public decimal SalePrice { get; set; }
    
    public decimal CurrentStock { get; set; }

    public DateTime? ExpirationDate { get; set; }
    
}