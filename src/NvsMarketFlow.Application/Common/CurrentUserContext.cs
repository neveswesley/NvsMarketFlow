namespace NvsMarketFlow.Application.Common;

public class CurrentUserContext : ICurrentUserContext
{
    public Guid? UserId { get; private set; }

    public void SetUserId(Guid userId)
    {
        UserId = userId;
    }
}