using NvsMarketFlow.Domain.Common;
using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Domain.Interfaces.ReadOnly;

public interface INotificationReadOnlyRepository
{
    Task<Notification?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<PagedResult<Notification>> GetAllAsync(
        Guid userId,
        bool? read,
        int page,
        int pageSize,
        CancellationToken ct);
}