using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Application.Responses.Sale;

public class FinalizeSaleResponse
{
    public Guid Id { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public SaleStatus Status { get; set; }
}