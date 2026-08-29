namespace NvsMarketFlow.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public string Token { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public string? ReplacedByToken { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public bool IsActive => RevokedAt is null && DateTime.UtcNow < ExpiresAt;

    public RefreshToken()
    {
    }

    public RefreshToken(Guid userId, string token, DateTime expiresAt)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Token = token;
        ExpiresAt = expiresAt;
        CreatedAt = DateTime.UtcNow;
    }

    public void Revoke(string? replacedByToken = null)
    {
        if (RevokedAt is not null)
            throw new InvalidOperationException("Refresh token is already revoked.");

        RevokedAt = DateTime.UtcNow;
        ReplacedByToken = replacedByToken;
    }
}