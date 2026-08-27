using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Application.Responses.Sale;

public class GetPaymentResponse
{
    public Guid Id { get; set; }
    public PaymentMethod Method { get; set; }
    public decimal Value { get; set; }
}