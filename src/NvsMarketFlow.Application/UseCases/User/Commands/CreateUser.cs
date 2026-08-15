using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Requests.User;
using NvsMarketFlow.Application.Responses.User;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.Services;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.User.Commands;

public class CreateUser
{
    public sealed record CreateUserCommand(CreateUserRequest Request) : IRequest<CreateUserResponse>;

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
    {
        private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserCommandHandler(IUserWriteOnlyRepository userWriteOnlyRepository, IUserReadOnlyRepository userReadOnlyRepository, IPasswordHasher passwordHasher)
        {
            _userWriteOnlyRepository = userWriteOnlyRepository;
            _userReadOnlyRepository = userReadOnlyRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var emailExists = await _userReadOnlyRepository.EmailExists(request.Request.Email, cancellationToken);

            if (emailExists)
                throw new DuplicateFieldException("User", "email", request.Request.Email);
            
            var passwordHash = _passwordHasher.Hash(request.Request.Password);

            var user = new Domain.Entities.User(request.Request.Name, request.Request.Email, passwordHash,
                request.Request.Role);

            await _userWriteOnlyRepository.CreateAsync(user, cancellationToken);

            return new CreateUserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}