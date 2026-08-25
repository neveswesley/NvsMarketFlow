using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Requests.User;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.User.Commands;

public class UpdateEmail
{
    public sealed record UpdateEmailCommand (Guid Id, UpdateEmailRequest Request) : IRequest<Unit>;

    public class UpdateEmailCommandHandler : IRequestHandler<UpdateEmailCommand, Unit>
    {
        private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;

        public UpdateEmailCommandHandler(IUserWriteOnlyRepository userWriteOnlyRepository, IUserReadOnlyRepository userReadOnlyRepository)
        {
            _userWriteOnlyRepository = userWriteOnlyRepository;
            _userReadOnlyRepository = userReadOnlyRepository;
        }

        public async Task<Unit> Handle(UpdateEmailCommand request, CancellationToken cancellationToken)
        {
            var emailExists = await _userReadOnlyRepository.EmailExists(request.Request.NewEmail, cancellationToken);
            if (emailExists)
                throw new DuplicateFieldException("Email", "email", request.Request.NewEmail);
            
            var user = await _userReadOnlyRepository.GetById(request.Id, cancellationToken);
            if (user == null)
                throw new NotFoundException("User not found");
            
            user.UpdateEmail(request.Request.NewEmail);
            await _userWriteOnlyRepository.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
            
        }
    }
}