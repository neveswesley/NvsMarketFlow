using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Requests.User;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.Services;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.User.Commands;

public class ChangePassword
{
    public sealed record ChangePasswordCommand (Guid Id, ChangePasswordRequest Request) : IRequest<Unit>;

    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
    {

        private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IPasswordHasher _passwordHasher;


        public ChangePasswordCommandHandler(IUserWriteOnlyRepository userWriteOnlyRepository, IUserReadOnlyRepository userReadOnlyRepository, IPasswordHasher passwordHasher)
        {
            _userWriteOnlyRepository = userWriteOnlyRepository;
            _userReadOnlyRepository = userReadOnlyRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userReadOnlyRepository.GetByIdAsync(request.Id, cancellationToken);
            if (user == null)
                throw new NotFoundException("User not found");

            var passwordHash = _passwordHasher.Hash(request.Request.NewPassword);
            
            user.ChangePassword(passwordHash);
            await _userWriteOnlyRepository.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}