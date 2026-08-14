namespace NvsMarketFlow.Domain.Entities;

public class Brand
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    public Brand(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public void Update(string name)
    {
        Name = name;
    }
    
}