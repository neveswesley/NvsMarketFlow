using NvsMarketFlow.Domain.Entities;

namespace NvsMarketFlow.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}