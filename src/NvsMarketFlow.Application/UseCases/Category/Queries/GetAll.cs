using MediatR;
using NvsMarketFlow.Application.Responses.Category;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;

namespace NvsMarketFlow.Application.UseCases.Category.Queries;

public abstract class GetAll
{
    public sealed record GetAllCategoriesQuery : IRequest<List<GetAllCategoryResponse>>;

    public sealed class
        GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<GetAllCategoryResponse>>
    {
        private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;

        public GetAllCategoriesQueryHandler(ICategoryReadOnlyRepository categoryReadOnlyRepository)
        {
            _categoryReadOnlyRepository = categoryReadOnlyRepository;
        }

        public async Task<List<GetAllCategoryResponse>> Handle(GetAllCategoriesQuery request,
            CancellationToken cancellationToken)
        {
            var categories = await _categoryReadOnlyRepository.GetAll();
            return categories.Select(c => new GetAllCategoryResponse()
            {
                Name = c.Name
            }).ToList();
        }
    }
}