namespace NvsMarketFlow.Domain.Entities;

public class SaleItem
{
    public Guid Id { get; private set; }

    public Guid SaleId { get; private set; }
    public Sale Sale { get; private set; } = null!;

    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;

    public decimal Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Discount { get; private set; }
    public decimal Total { get; private set; }

    public SaleItem()
    {
    }

    public SaleItem(Guid saleId, Guid productId, decimal quantity, decimal unitPrice, decimal discount)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative.");

        if (discount < 0)
            throw new ArgumentException("Discount cannot be negative.");

        var total = (quantity * unitPrice) - discount;

        if (total < 0)
            throw new ArgumentException("Discount cannot be greater than the item subtotal.");

        Id = Guid.NewGuid();
        SaleId = saleId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = discount;
        Total = total;
    }
}