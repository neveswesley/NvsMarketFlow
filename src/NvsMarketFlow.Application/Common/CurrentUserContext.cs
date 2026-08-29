namespace NvsMarketFlow.Application.Common;

public class CurrentUserContext : ICurrentUserContext
{
    public Guid? UserId { get; private set; }
    public string? Role { get; private set; }

    public void SetUserId(Guid userId) => UserId = userId;
    public void SetRole(string role) => Role = role;

    public bool IsOwnerOrAdmin(Guid resourceOwnerId)
    {
        if (Role == "Administrator")
            return true;

        return UserId == resourceOwnerId;
    }
}