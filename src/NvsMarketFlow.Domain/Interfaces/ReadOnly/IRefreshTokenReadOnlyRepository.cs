using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Domain.Interfaces.ReadOnly;

public interface IRefreshTokenReadOnlyRepository
{
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct);
}