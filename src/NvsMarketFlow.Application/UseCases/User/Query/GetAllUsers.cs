using MediatR;
using NvsMarketFlow.Application.Common;
using NvsMarketFlow.Application.Responses.Brand;
using NvsMarketFlow.Application.Responses.User;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;

namespace NvsMarketFlow.Application.UseCases.User.Query;

public class GetAllUsers
{
    public sealed record GetAllUsersQuery (string? Name, int Page = 1, int PageSize = 10) : IRequest<PagedResult<GetUserResponse>>;
    
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PagedResult<GetUserResponse>>
    {
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;

        public GetAllUsersQueryHandler(IUserReadOnlyRepository userReadOnlyRepository)
        {
            _userReadOnlyRepository = userReadOnlyRepository;
        }

        public async Task<PagedResult<GetUserResponse>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var result = await _userReadOnlyRepository.GetAllAsync(
                request.Name,
                request.Page,
                request.PageSize,
                cancellationToken);

            var items = result.Items.Select(u => new GetUserResponse
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role,
                UserStatus = u.Status
            }).ToList();

            return new PagedResult<GetUserResponse>(items, result.Page, result.PageSize, result.TotalItems,
                result.TotalPages);
            
        }
    }
}