using Microsoft.EntityFrameworkCore;
using NvsMarketFlow.Domain.Entities;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;
using NvsMarketFlow.Infrastructure.DataAccess;

namespace NvsMarketFlow.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenWriteOnlyRepository, IRefreshTokenReadOnlyRepository
{
    private readonly AppDbContext _dbContext;

    public RefreshTokenRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RefreshToken> CreateAsync(RefreshToken refreshToken, CancellationToken ct)
    {
        await _dbContext.RefreshTokens.AddAsync(refreshToken, ct);
        return refreshToken;
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct)
    {
        return await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token, ct);
    }
    
    public async Task RevokeAllByUserIdAsync(Guid userId, CancellationToken ct)
    {
        var activeTokens = await _dbContext.RefreshTokens
            .Where(rt => rt.UserId == userId && rt.RevokedAt == null)
            .ToListAsync(ct);

        foreach (var token in activeTokens)
        {
            token.Revoke();
        }
    }
}