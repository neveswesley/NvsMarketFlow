using NvsMarketFlow.Domain.Common;
using NvsMarketFlow.Domain.Entities;
using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Domain.Interfaces.ReadOnly;

public interface IProductReadOnlyRepository
{
    Task<Product?> GetByBarcodeAsync(string barcode, CancellationToken ct);
    Task<bool> ExistsByNameAsync(string name, Guid? excludeId, CancellationToken ct);
    Task<bool> ExistsBySkuAsync(string sku, Guid? excludeId, CancellationToken ct);
    Task<bool> ExistsByBarcodeAsync(string barcode, Guid? excludeId, CancellationToken ct);

    Task<PagedResult<Product>> GetAllAsync(
        string? name,
        Guid? categoryId,
        Guid? brandId,
        Status? status,
        decimal? minPrice,
        decimal? maxPrice,
        bool? lowStock,
        int page,
        int pageSize,
        CancellationToken ct);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken ct);
}