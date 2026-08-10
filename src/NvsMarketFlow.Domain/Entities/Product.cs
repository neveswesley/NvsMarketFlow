using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }

    public string Sku { get; private set; } = string.Empty;
    public string Barcode { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;


    public Guid? BrandId { get; private set; }
    public Brand? Brand { get; private set; }

    public decimal CostPrice { get; private set; }
    public decimal SalePrice { get; private set; }


    public decimal CurrentStock { get; private set; }
    public decimal MinimumStock { get; private set; }
    public decimal MaximumStock { get; private set; }

    public DateTime? ExpirationDate { get; private set; }

    public Unit Unit { get; private set; }

    public Status Status { get; private set; }


    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public Product()
    {
        
    }

    public Product(string sku, string name, string description, Guid categoryId, Guid? brandId, decimal costPrice, decimal salePrice,
    decimal currentStock, decimal minimumStock, decimal maximumStock, DateTime? expirationDate, Unit unit, Status status)
    {
        Id = Guid.NewGuid();
        Sku = sku;
        Barcode = GenerateBarCode();
        Name = name;
        Description = description;
        CategoryId = categoryId;
        BrandId = brandId;
        CostPrice = costPrice;
        SalePrice = salePrice;
        CurrentStock = currentStock;
        MinimumStock = minimumStock;
        MaximumStock = maximumStock;
        ExpirationDate = expirationDate;
        Unit =  unit;
        Status = status;
        CreatedAt = DateTime.UtcNow;
    }

    private static string GenerateBarCode()
    {
        var barCode = GenerateBarCode();
        return barCode;
    }
    
}