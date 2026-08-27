using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Domain.Entities;

public class CashRegister
{
    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public DateTime OpenedAt { get; private set; }
    public DateTime? ClosedAt { get; private set; }

    public decimal OpeningBalance { get; private set; }
    public decimal? ClosingBalance { get; private set; }

    public CashRegisterStatus Status { get; private set; }

    public CashRegister()
    {
    }

    public CashRegister(Guid userId, decimal openingBalance)
    {
        if (openingBalance < 0)
            throw new ArgumentException("Opening balance cannot be negative.");

        Id = Guid.NewGuid();
        UserId = userId;
        OpeningBalance = openingBalance;
        Status = CashRegisterStatus.Open;
        OpenedAt = DateTime.UtcNow;
    }

    public decimal Close(decimal closingBalance, decimal expectedBalance)
    {
        if (Status == CashRegisterStatus.Closed)
            throw new InvalidOperationException("Cash register is already closed.");

        if (closingBalance < 0)
            throw new ArgumentException("Closing balance cannot be negative.");

        ClosingBalance = closingBalance;
        Status = CashRegisterStatus.Closed;
        ClosedAt = DateTime.UtcNow;

        return closingBalance - expectedBalance;
    }
}