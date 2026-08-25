namespace NvsMarketFlow.Domain.Interfaces.WriteOnly;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct);
}