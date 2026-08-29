using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Interfaces;
using NvsMarketFlow.Application.Requests.Auth;
using NvsMarketFlow.Application.Responses.Auth;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Auth.Commands;

public class RefreshAccessToken
{
    public sealed record RefreshAccessTokenCommand(RefreshTokenRequest Request) : IRequest<LoginResponse>;

    public class RefreshAccessTokenCommandHandler : IRequestHandler<RefreshAccessTokenCommand, LoginResponse>
    {
        private readonly IRefreshTokenReadOnlyRepository _refreshTokenReadOnlyRepository;
        private readonly IRefreshTokenWriteOnlyRepository _refreshTokenWriteOnlyRepository;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshAccessTokenCommandHandler(
            IRefreshTokenReadOnlyRepository refreshTokenReadOnlyRepository,
            IRefreshTokenWriteOnlyRepository refreshTokenWriteOnlyRepository,
            IUserReadOnlyRepository userReadOnlyRepository,
            ITokenService tokenService,
            IUnitOfWork unitOfWork)
        {
            _refreshTokenReadOnlyRepository = refreshTokenReadOnlyRepository;
            _refreshTokenWriteOnlyRepository = refreshTokenWriteOnlyRepository;
            _userReadOnlyRepository = userReadOnlyRepository;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }

        public async Task<LoginResponse> Handle(RefreshAccessTokenCommand command, CancellationToken ct)
        {
            var existingToken = await _refreshTokenReadOnlyRepository
                .GetByTokenAsync(command.Request.RefreshToken, ct);

            if (existingToken is null || !existingToken.IsActive)
                throw new UnauthorizedException("Invalid or expired refresh token.");

            var user = await _userReadOnlyRepository.GetByIdAsync(existingToken.UserId, ct);

            if (user is null)
                throw new UnauthorizedException("User not found.");

            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshTokenValue = _tokenService.GenerateRefreshToken();
            var expiresAt = DateTime.UtcNow.AddDays(7);

            var newRefreshToken = new Domain.Entities.RefreshToken(user.Id, newRefreshTokenValue, expiresAt);

            // Rotação: revoga o antigo, aponta pro novo — se esse token velho
            // aparecer de novo no futuro, é sinal de token roubado sendo reusado.
            existingToken.Revoke(newRefreshTokenValue);

            await _refreshTokenWriteOnlyRepository.CreateAsync(newRefreshToken, ct);

            await _unitOfWork.SaveChangesAsync(ct);

            return new LoginResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshTokenValue,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15)
            };
        }
    }
}