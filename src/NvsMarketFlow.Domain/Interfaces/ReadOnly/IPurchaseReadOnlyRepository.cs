using NvsMarketFlow.Domain.Common;
using NvsMarketFlow.Domain.Entities;
using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Domain.Interfaces.ReadOnly;

public interface IPurchaseReadOnlyRepository
{
    Task<bool> ExistsByInvoiceNumberAsync(string invoiceNumber, Guid? excludeId, CancellationToken ct);
    Task<Purchase?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<PagedResult<Purchase>> GetAllAsync(
        Guid? supplierId,
        string? invoiceNumber,
        PurchaseStatus? status,
        DateTime? startDate,
        DateTime? endDate,
        int page,
        int pageSize,
        CancellationToken ct);
}