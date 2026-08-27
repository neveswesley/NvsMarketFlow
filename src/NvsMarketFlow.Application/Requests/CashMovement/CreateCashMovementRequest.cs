using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Application.Requests.CashMovement;

public class CreateCashMovementRequest
{
    public CashMovementType Type { get; set; }
    public decimal Value { get; set; }
    public string Reason { get; set; } = string.Empty;
}