using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Application.Responses.Sale;

public class AddPaymentResponse
{
    public Guid SaleId { get; set; }
    public PaymentMethod Method { get; set; }
    public decimal Value { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal RemainingAmount { get; set; }
}