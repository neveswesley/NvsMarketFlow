using MediatR;
using NvsMarketFlow.Application.Responses.Brand;
using NvsMarketFlow.Domain.Common;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;

namespace NvsMarketFlow.Application.UseCases.Brand.Queries;

public class GetAllBrands
{
    public sealed record GetAllBrandsQuery(string? Name, int Page = 1, int PageSize = 10)
        : IRequest<PagedResult<GetBrandResponse>>;

    public class GetAllBrandsQueryHandler : IRequestHandler<GetAllBrandsQuery, PagedResult<GetBrandResponse>>
    {
        private readonly IBrandReadOnlyRepository _brandReadOnlyRepository;

        public GetAllBrandsQueryHandler(IBrandReadOnlyRepository brandReadOnlyRepository)
        {
            _brandReadOnlyRepository = brandReadOnlyRepository;
        }

        public async Task<PagedResult<GetBrandResponse>> Handle(GetAllBrandsQuery request,
            CancellationToken cancellationToken)
        {
            var result =
                await _brandReadOnlyRepository.GetAllAsync(request.Name, request.Page, request.PageSize,
                    cancellationToken);

            var items = result.Items.Select(b => new GetBrandResponse()
            {
                Name = b.Name,
            }).ToList();

            return new PagedResult<GetBrandResponse>(items, result.Page, result.PageSize, result.TotalItems,
                result.TotalPages);
        }
    }
}