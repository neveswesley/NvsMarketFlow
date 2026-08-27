using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Application.Responses.CashMovement;

public class GetCashMovementResponse
{
    public Guid Id { get; set; }
    public CashMovementType Type { get; set; }
    public decimal Value { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}