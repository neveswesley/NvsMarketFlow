using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Domain.Interfaces.WriteOnly;

public interface IUserWriteOnlyRepository
{
    Task<User> CreateAsync(User user, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken ct);
}