using MediatR;
using NvsMarketFlow.Application.Common;
using NvsMarketFlow.Application.Responses.Category;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;

namespace NvsMarketFlow.Application.UseCases.Category.Queries;

public abstract class GetAllCategory
{
    public sealed record GetAllCategoriesQuery (string? Name, int Page = 1, int PageSize = 10) : IRequest<PagedResult<GetAllCategoryResponse>>;

    public sealed class
        GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, PagedResult<GetAllCategoryResponse>>
    {
        private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;

        public GetAllCategoriesQueryHandler(ICategoryReadOnlyRepository categoryReadOnlyRepository)
        {
            _categoryReadOnlyRepository = categoryReadOnlyRepository;
        }

        public async Task<PagedResult<GetAllCategoryResponse>> Handle(GetAllCategoriesQuery request,
            CancellationToken ct)
        {
            
            var result = await _categoryReadOnlyRepository.GetAllAsync(
                request.Name,
                request.Page,
                request.PageSize,
                ct);

            var items = result.Items
                .Select(c => new GetAllCategoryResponse
                {
                    Name = c.Name
                })
                .ToList();
            
            return new PagedResult<GetAllCategoryResponse>(
                items,
                result.Page,
                result.PageSize,
                result.TotalItems,
                result.TotalPages);
        }
    }
}