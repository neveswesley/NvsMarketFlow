using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Application.Requests.Sale;

public class AddPaymentRequest
{
    public PaymentMethod Method { get; set; }
    public decimal Value { get; set; }
}