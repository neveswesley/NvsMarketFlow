using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Application.Responses.CashRegister;

public class CloseCashRegisterResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal OpeningBalance { get; set; }
    public decimal ExpectedBalance { get; set; }
    public decimal ClosingBalance { get; set; }
    public decimal Discrepancy { get; set; }
    public CashRegisterStatus Status { get; set; }
    public DateTime OpenedAt { get; set; }
    public DateTime ClosedAt { get; set; }
}