namespace NvsMarketFlow.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public ICollection<Product> Products { get; set; } = [];

    public Category(string name)
    {
        Name = name;
    }

    public void UpdateCategory(string name)
    {
        Name = name;
    }
}