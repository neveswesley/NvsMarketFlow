using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Domain.Interfaces.WriteOnly;

public interface IRefreshTokenWriteOnlyRepository
{
    Task<RefreshToken> CreateAsync(RefreshToken refreshToken, CancellationToken ct);
    Task RevokeAllByUserIdAsync(Guid userId, CancellationToken ct);
}