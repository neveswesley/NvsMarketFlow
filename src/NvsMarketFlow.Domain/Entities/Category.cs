namespace NvsMarketFlow.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public ICollection<Product> Products { get; set; } = [];

    public Category(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty;");
        
        Id = Guid.NewGuid();
        Name = name;
    }

    public void UpdateCategory(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty;");
        
        Name = name;
    }
}