using MediatR;
using NvsMarketFlow.Application.Requests.Auth;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Auth.Commands;

public class Logout
{
    public sealed record LogoutCommand(LogoutRequest Request) : IRequest;

    public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
    {
        private readonly IRefreshTokenReadOnlyRepository _refreshTokenReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LogoutCommandHandler(
            IRefreshTokenReadOnlyRepository refreshTokenReadOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _refreshTokenReadOnlyRepository = refreshTokenReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(LogoutCommand command, CancellationToken ct)
        {
            var token = await _refreshTokenReadOnlyRepository.GetByTokenAsync(command.Request.RefreshToken, ct);

            // Se não achar, não é erro — apenas não há nada pra revogar.
            // Não vazamos informação sobre validade do token.
            if (token is not null && token.IsActive)
            {
                token.Revoke();
                await _unitOfWork.SaveChangesAsync(ct);
            }
        }
    }
}