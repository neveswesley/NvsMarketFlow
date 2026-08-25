namespace NvsMarketFlow.Application.Responses.Supplier;

public class CreateSupplierResponse
{
    public Guid Id { get; set; }
    public string CorporateName { get; set; } = string.Empty;
    public string FantasyName { get; set; } = string.Empty;
    public string CNPJ { get; set; } = string.Empty;
}