using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Domain.Entities;

public class Payment
{
    public Guid Id { get; private set; }

    public Guid SaleId { get; private set; }
    public Sale Sale { get; private set; } = null!;

    public PaymentMethod Method { get; private set; }
    public decimal Value { get; private set; }

    public Payment()
    {
    }

    public Payment(Guid saleId, PaymentMethod method, decimal value)
    {
        if (value <= 0)
            throw new ArgumentException("Value must be greater than zero.");

        Id = Guid.NewGuid();
        SaleId = saleId;
        Method = method;
        Value = value;
    }
}