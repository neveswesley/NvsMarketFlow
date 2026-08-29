using MediatR;
using NvsMarketFlow.Application.Common;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Requests.User;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.Services;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.User.Commands;

public class ChangePassword
{
    public sealed record ChangePasswordCommand(Guid Id, ChangePasswordRequest Request) : IRequest<Unit>;

    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
    {
        private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRefreshTokenWriteOnlyRepository _refreshTokenWriteOnlyRepository;
        private readonly ICurrentUserContext _currentUserContext;

        public ChangePasswordCommandHandler(IUserWriteOnlyRepository userWriteOnlyRepository, IUserReadOnlyRepository userReadOnlyRepository, IPasswordHasher passwordHasher, IRefreshTokenWriteOnlyRepository refreshTokenWriteOnlyRepository, ICurrentUserContext currentUserContext)
        {
            _userWriteOnlyRepository = userWriteOnlyRepository;
            _userReadOnlyRepository = userReadOnlyRepository;
            _passwordHasher = passwordHasher;
            _refreshTokenWriteOnlyRepository = refreshTokenWriteOnlyRepository;
            _currentUserContext = currentUserContext;
        }

        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userReadOnlyRepository.GetByIdAsync(request.Id, cancellationToken);

            if (user is null)
                throw new NotFoundException($"User with id '{request.Id}' not found.");

            if (!_currentUserContext.IsOwnerOrAdmin(user.Id))
                throw new ForbiddenException("You can only change your own password.");

            var passwordHash = _passwordHasher.Hash(request.Request.NewPassword);

            user.ChangePassword(passwordHash);
            
            await _refreshTokenWriteOnlyRepository.RevokeAllByUserIdAsync(user.Id, cancellationToken);
            await _userWriteOnlyRepository.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}