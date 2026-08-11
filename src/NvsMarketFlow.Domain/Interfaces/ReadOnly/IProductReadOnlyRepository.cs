namespace NvsMarketFlow.Domain.Interfaces.ReadOnly;

public interface IProductReadOnlyRepository
{
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct);
    Task<bool> ExistsBySkuAsync(string sku, CancellationToken ct);
    Task<bool> ExistsByBarcodeAsync(string barcode, CancellationToken ct);
    

}