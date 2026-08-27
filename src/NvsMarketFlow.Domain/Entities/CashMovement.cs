using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Domain.Entities;

public class CashMovement
{
    public Guid Id { get; private set; }

    public Guid CashRegisterId { get; private set; }
    public CashRegister CashRegister { get; private set; } = null!;

    public CashMovementType Type { get; private set; }
    public decimal Value { get; private set; }
    public string Reason { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public CashMovement()
    {
    }

    public CashMovement(Guid cashRegisterId, CashMovementType type, decimal value, string reason)
    {
        if (value <= 0)
            throw new ArgumentException("Value must be greater than zero.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reason cannot be empty;");

        Id = Guid.NewGuid();
        CashRegisterId = cashRegisterId;
        Type = type;
        Value = value;
        Reason = reason;
        CreatedAt = DateTime.UtcNow;
    }

    public bool IsIncrease => Type switch
    {
        CashMovementType.Supply => true,
        CashMovementType.Sale => true,
        CashMovementType.Withdrawal => false,
        CashMovementType.Cancellation => false,
        CashMovementType.Change => false,
        _ => throw new ArgumentOutOfRangeException(nameof(Type), Type, "Unmapped cash movement type.")
    };
}