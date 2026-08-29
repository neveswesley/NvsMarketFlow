namespace NvsMarketFlow.Application.Common;

public interface ICurrentUserContext
{
    Guid? UserId { get; }
    string? Role { get; }
    void SetUserId(Guid userId);
    void SetRole(string role);

    bool IsOwnerOrAdmin(Guid resourceOwnerId);
}