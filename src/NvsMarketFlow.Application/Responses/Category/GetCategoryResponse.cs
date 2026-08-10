namespace NvsMarketFlow.Application.Responses.Category;

public class GetCategoryResponse
{
    public string Name { get; set; } = string.Empty;
    public List<GetCategoryProductResponse> Products { get; set; } = [];
}