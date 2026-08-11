using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }

    public string Sku { get; private set; } = string.Empty;
    public string Barcode { get; private set; } = string.Empty;    public string Name { get; private set; } = string.Empty;


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
        
        if (costPrice < 0)
            throw new ArgumentException("Cost price cannot be negative.");

        if (salePrice <= 0)
            throw new ArgumentException("Sale price must be greater than zero.");

        if (currentStock < 0)
            throw new ArgumentException("Current stock cannot be negative.");

        if (minimumStock < 0)
            throw new ArgumentException("Minimum stock cannot be negative.");

        if (maximumStock <= 0)
            throw new ArgumentException("Maximum stock must be greater than zero.");

        if (maximumStock < minimumStock)
            throw new ArgumentException("Maximum stock cannot be lower than minimum stock.");

        if (currentStock > maximumStock)
            throw new ArgumentException("Current stock cannot exceed maximum stock.");
        
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
        var random = new Random();
        var digits = new int[13];

        // Gera os primeiros 12 dígitos aleatoriamente
        for (int i = 0; i < 12; i++)
        {
            digits[i] = random.Next(0, 10);
        }

        // Calcula o dígito verificador (13º dígito)
        int sum = 0;
        for (int i = 0; i < 12; i++)
        {
            // Posições pares (índice 0,2,4...) peso 1, ímpares (1,3,5...) peso 3
            sum += digits[i] * (i % 2 == 0 ? 1 : 3);
        }

        int checkDigit = (10 - (sum % 10)) % 10;
        digits[12] = checkDigit;

        return string.Concat(digits);
    }
    
}