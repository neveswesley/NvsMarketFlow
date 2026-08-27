using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Application.Responses.CashRegister;

public class OpenCashRegisterResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal OpeningBalance { get; set; }
    public CashRegisterStatus Status { get; set; }
    public DateTime OpenedAt { get; set; }
}