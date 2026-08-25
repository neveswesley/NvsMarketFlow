using NvsMarketFlow.Application.Common;
using NvsMarketFlow.Domain.Entities;
using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Domain.Interfaces.ReadOnly;

public interface ISupplierReadOnlyRepository
{
    Task<bool> ExistsByCorporateNameAsync(string corporateName, Guid? excludeId, CancellationToken ct);
    Task<bool> ExistsByCnpjAsync(string cnpj, Guid? excludeId, CancellationToken ct);
    Task<Supplier?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<PagedResult<Supplier>> GetAllAsync(
        string? corporateName,
        string? fantasyName,
        string? cnpj,
        Status? status,
        int page,
        int pageSize,
        CancellationToken ct);
}