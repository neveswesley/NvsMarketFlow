using MediatR;
using NvsMarketFlow.Application.Common;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Interfaces;
using NvsMarketFlow.Application.Requests.Auth;
using NvsMarketFlow.Application.Responses.Auth;
using NvsMarketFlow.Domain.Enums;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;
using NvsMarketFlow.Domain.Interfaces.Services;

namespace NvsMarketFlow.Application.UseCases.Auth.Commands;

public class Login
{
    public sealed record LoginCommand(LoginRequest Request) : IRequest<LoginResponse>;

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IRefreshTokenWriteOnlyRepository _refreshTokenWriteOnlyRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public LoginCommandHandler(
            IUserReadOnlyRepository userReadOnlyRepository,
            IRefreshTokenWriteOnlyRepository refreshTokenWriteOnlyRepository,
            IPasswordHasher passwordHasher,
            ITokenService tokenService,
            IUnitOfWork unitOfWork)
        {
            _userReadOnlyRepository = userReadOnlyRepository;
            _refreshTokenWriteOnlyRepository = refreshTokenWriteOnlyRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }

        public async Task<LoginResponse> Handle(LoginCommand command, CancellationToken ct)
        {
            var user = await _userReadOnlyRepository.GetByEmail(command.Request.Email, ct);

            if (user is null || !_passwordHasher.Verify(command.Request.Password, user.PasswordHash))
                throw new UnauthorizedException("Invalid email or password.");

            if (user.Status != UserStatus.Active)
                throw new UnauthorizedException("User is inactive.");

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshTokenValue = _tokenService.GenerateRefreshToken();
            var expiresAt = DateTime.UtcNow.AddDays(7); // idealmente ler de JwtSettings

            var refreshToken = new Domain.Entities.RefreshToken(user.Id, refreshTokenValue, expiresAt);

            await _refreshTokenWriteOnlyRepository.CreateAsync(refreshToken, ct);

            user.RegisterLogin();

            await _unitOfWork.SaveChangesAsync(ct);

            return new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15) // idealmente ler de JwtSettings
            };
        }
    }
}