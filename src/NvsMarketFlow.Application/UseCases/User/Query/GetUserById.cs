using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Responses.User;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;

namespace NvsMarketFlow.Application.UseCases.User.Query;

public class GetUserById
{
    public sealed record GetUserByIdQuery(Guid Id, CancellationToken CancellationToken) : IRequest<GetUserResponse>;

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GetUserResponse>
    {
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;

        public GetUserByIdQueryHandler(IUserReadOnlyRepository userReadOnlyRepository)
        {
            _userReadOnlyRepository = userReadOnlyRepository;
        }

        public async Task<GetUserResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userReadOnlyRepository.GetByIdAsync(request.Id, request.CancellationToken);

            if (user == null)
                throw new NotFoundException("User not found.");

            return new GetUserResponse()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                UserStatus = user.Status,
            };
        }
    }
}