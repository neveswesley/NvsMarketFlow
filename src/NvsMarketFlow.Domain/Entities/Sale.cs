using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Domain.Entities;

public class Sale
{
    public Guid Id { get; private set; }

    public Guid CashRegisterId { get; private set; }
    public CashRegister CashRegister { get; private set; } = null!;

    public Guid SellerId { get; private set; }
    public User Seller { get; private set; } = null!;

    public string SaleNumber { get; private set; }

    public decimal Subtotal { get; private set; }
    public decimal Discount { get; private set; }
    public decimal Total { get; private set; }

    public SaleStatus Status { get; private set; }

    public ICollection<SaleItem> Items { get; private set; } = new List<SaleItem>();
    public ICollection<Payment> Payments { get; private set; } = new List<Payment>();

    public DateTime CreatedAt { get; private set; }

    public Sale()
    {
    }

    public Sale(Guid cashRegisterId, Guid sellerId, string saleNumber)
    {
        if (string.IsNullOrWhiteSpace(saleNumber))
            throw new ArgumentException("Sale number cannot be empty;");

        Id = Guid.NewGuid();
        CashRegisterId = cashRegisterId;
        SellerId = sellerId;
        SaleNumber = saleNumber;
        Subtotal = 0;
        Discount = 0;
        Total = 0;
        Status = SaleStatus.Open;
        CreatedAt = DateTime.UtcNow;
    }

    public SaleItem AddItem(Guid productId, decimal quantity, decimal unitPrice, decimal discount)
    {
        if (Status != SaleStatus.Open)
            throw new InvalidOperationException("Cannot add items to a sale that is not open.");

        var item = new SaleItem(Id, productId, quantity, unitPrice, discount);

        Items.Add(item);

        RecalculateTotals();

        return item;
    }

    public void RemoveItem(Guid saleItemId)
    {
        if (Status != SaleStatus.Open)
            throw new InvalidOperationException("Cannot remove items from a sale that is not open.");

        var item = Items.FirstOrDefault(i => i.Id == saleItemId);

        if (item is null)
            throw new InvalidOperationException("Item not found in this sale.");

        Items.Remove(item);

        RecalculateTotals();
    }

    public decimal TotalPaid => Payments.Sum(p => p.Value);
    public decimal RemainingAmount => Total - TotalPaid;

    public Payment AddPayment(PaymentMethod method, decimal value)
    {
        if (Status != SaleStatus.Open)
            throw new InvalidOperationException("Cannot add payments to a sale that is not open.");

        if (!Items.Any())
            throw new InvalidOperationException("Cannot add a payment to a sale without items.");

        if (value > RemainingAmount)
            throw new InvalidOperationException(
                $"Payment value ({value}) exceeds the remaining amount ({RemainingAmount}) for this sale.");

        var payment = new Payment(Id, method, value);

        Payments.Add(payment);

        return payment;
    }

    public void Finalize()
    {
        if (Status != SaleStatus.Open)
            throw new InvalidOperationException("Only open sales can be finalized.");

        if (!Items.Any())
            throw new InvalidOperationException("Cannot finalize a sale without items.");

        if (RemainingAmount != 0)
            throw new InvalidOperationException(
                $"Cannot finalize sale with remaining amount of {RemainingAmount}. Sale must be fully paid.");

        Status = SaleStatus.Completed;
    }
    
    public void Cancel()
    {
        if (Status != SaleStatus.Open)
            throw new InvalidOperationException("Only open sales can be cancelled.");

        Status = SaleStatus.Cancelled;
    }

    private void RecalculateTotals()
    {
        Subtotal = Items.Sum(i => i.Quantity * i.UnitPrice);
        Discount = Items.Sum(i => i.Discount);
        Total = Items.Sum(i => i.Total);
    }
}