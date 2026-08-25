using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Application.Responses.Supplier;

public class GetSupplierByIdResponse
{
    public Guid Id { get; set; }
    public string CorporateName { get; set; } = string.Empty;
    public string FantasyName { get; set; } = string.Empty;
    public string CNPJ { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public Status Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}