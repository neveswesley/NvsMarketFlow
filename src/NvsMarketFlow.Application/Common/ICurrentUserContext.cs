namespace NvsMarketFlow.Application.Common;

public interface ICurrentUserContext
{
    Guid? UserId { get; }
    void SetUserId(Guid userId);
}