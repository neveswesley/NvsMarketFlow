namespace NvsMarketFlow.Domain.Entities;

public class PurchaseItem
{
    public Guid Id { get; private set; }

    public Guid PurchaseId { get; private set; }
    public Purchase Purchase { get; private set; } = null!;

    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;

    public decimal Quantity { get; private set; }
    public decimal CostPrice { get; private set; }

    public PurchaseItem()
    {
    }

    public PurchaseItem(Guid purchaseId, Guid productId, decimal quantity, decimal costPrice)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        if (costPrice < 0)
            throw new ArgumentException("Cost price cannot be negative.");

        Id = Guid.NewGuid();
        PurchaseId = purchaseId;
        ProductId = productId;
        Quantity = quantity;
        CostPrice = costPrice;
    }
}