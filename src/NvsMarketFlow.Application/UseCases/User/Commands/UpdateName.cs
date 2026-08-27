using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Requests.User;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.User.Commands;

public class UpdateName
{
    public sealed record UpdateNameCommand (Guid Id, UpdateNameRequest Request) : IRequest<Unit>;

    public class UpdateNameCommandHandler : IRequestHandler<UpdateNameCommand, Unit>
    {
        
        private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;

        public UpdateNameCommandHandler(IUserWriteOnlyRepository userWriteOnlyRepository, IUserReadOnlyRepository userReadOnlyRepository)
        {
            _userWriteOnlyRepository = userWriteOnlyRepository;
            _userReadOnlyRepository = userReadOnlyRepository;
        }

        public async Task<Unit> Handle(UpdateNameCommand request, CancellationToken cancellationToken)
        {
            var user = await _userReadOnlyRepository.GetByIdAsync(request.Id, cancellationToken);
            if (user == null)
                throw new NotFoundException("User not found.");
            
            user.UpdateName(request.Request.NewName);
            await _userWriteOnlyRepository.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}