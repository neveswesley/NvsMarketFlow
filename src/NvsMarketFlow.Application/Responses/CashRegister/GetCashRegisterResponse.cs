using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Application.Responses.CashRegister;

public class GetCashRegisterResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public decimal OpeningBalance { get; set; }
    public decimal? ClosingBalance { get; set; }
    public CashRegisterStatus Status { get; set; }
    public DateTime OpenedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
}