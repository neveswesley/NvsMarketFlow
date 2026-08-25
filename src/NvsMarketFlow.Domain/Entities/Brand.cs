namespace NvsMarketFlow.Domain.Entities;

public class Brand
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    public Brand(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Brand name is required.", nameof(name));
        
        Id = Guid.NewGuid();
        Name = name;
    }

    public void Update(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Brand name is required.", nameof(name));
        
        Name = name;
    }
    
}