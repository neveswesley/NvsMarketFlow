namespace NvsMarketFlow.Application.Responses.Product;

public class CreateProductResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal SalePrice { get; set; }
    public Guid CategoryId { get; set; }
}