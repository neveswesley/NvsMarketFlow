namespace NvsMarketFlow.Application.Requests.CashRegister;

public class OpenCashRegisterRequest
{
    public Guid UserId { get; set; }
    public decimal OpeningBalance { get; set; }
}