using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.User.Commands;

public class DeleteUser
{
    public sealed record DeleteUserCommand (Guid Id) : IRequest<Unit>;
    
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
    {

        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;

        public DeleteUserCommandHandler(IUserReadOnlyRepository userReadOnlyRepository, IUserWriteOnlyRepository userWriteOnlyRepository)
        {
            _userReadOnlyRepository = userReadOnlyRepository;
            _userWriteOnlyRepository = userWriteOnlyRepository;
        }
        
        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userReadOnlyRepository.GetById(request.Id, cancellationToken);
            if (user == null)
                throw new NotFoundException("User not found.");
            
            user.Deactivate();
            
            await _userWriteOnlyRepository.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}