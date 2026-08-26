using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Domain.Entities;

public class Purchase
{
    public Guid Id { get; private set; }

    public Guid SupplierId { get; private set; }
    public Supplier Supplier { get; private set; } = null!;

    public string InvoiceNumber { get; private set; }
    public decimal Total { get; private set; }

    public PurchaseStatus Status { get; private set; }

    public ICollection<PurchaseItem> Items { get; private set; } = new List<PurchaseItem>();

    public DateTime CreatedAt { get; private set; }

    public Purchase()
    {
    }

    public Purchase(Guid supplierId, string invoiceNumber)
    {
        if (string.IsNullOrWhiteSpace(invoiceNumber))
            throw new ArgumentException("Invoice number cannot be empty;");

        Id = Guid.NewGuid();
        SupplierId = supplierId;
        InvoiceNumber = invoiceNumber;
        Total = 0;
        Status = PurchaseStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public PurchaseItem AddItem(Guid productId, decimal quantity, decimal costPrice)
    {
        if (Status != PurchaseStatus.Pending)
            throw new InvalidOperationException("Cannot add items to a purchase that is not pending.");

        var item = new PurchaseItem(Id, productId, quantity, costPrice);

        Items.Add(item);

        RecalculateTotal();

        return item;
    }
    
    public void RemoveItem(Guid purchaseItemId)
    {
        if (Status != PurchaseStatus.Pending)
            throw new InvalidOperationException("Cannot remove items from a purchase that is not pending.");

        var item = Items.FirstOrDefault(i => i.Id == purchaseItemId);

        if (item is null)
            throw new InvalidOperationException("Item not found in this purchase.");

        Items.Remove(item);

        RecalculateTotal();
    }

    private void RecalculateTotal()
    {
        Total = Items.Sum(i => i.Quantity * i.CostPrice);
    }
    
    public void Confirm()
    {
        if (Status != PurchaseStatus.Pending)
            throw new InvalidOperationException("Only pending purchases can be confirmed.");

        if (!Items.Any())
            throw new InvalidOperationException("Cannot confirm a purchase without items.");

        Status = PurchaseStatus.Confirmed;
    }
}