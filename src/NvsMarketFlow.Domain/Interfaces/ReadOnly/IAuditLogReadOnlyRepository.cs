using System.Net.Mime;
using NvsMarketFlow.Domain.Common;
using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Domain.Interfaces.ReadOnly;

public interface IAuditLogReadOnlyRepository
{
    Task<PagedResult<AuditLog>> GetAllAsync(
        Guid? userId,
        string? entity,
        string? action,
        DateTime? startDate,
        DateTime? endDate,
        int page,
        int pageSize,
        CancellationToken ct);
}