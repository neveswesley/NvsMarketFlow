using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Domain.Interfaces.WriteOnly;

public interface INotificationWriteOnlyRepository
{
    Task<Notification> CreateAsync(Notification notification, CancellationToken ct);
}