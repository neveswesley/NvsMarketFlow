using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Responses.Category;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;

namespace NvsMarketFlow.Application.UseCases.Category.Queries;

public abstract class GetCategoryById
{
    public sealed record GetByIdQuery(Guid CategoryId) : IRequest<GetCategoryResponse>;

    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, GetCategoryResponse>
    {
        private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;

        public GetByIdQueryHandler(ICategoryReadOnlyRepository categoryReadOnlyRepository)
        {
            _categoryReadOnlyRepository = categoryReadOnlyRepository;
        }

        public async Task<GetCategoryResponse> Handle(GetByIdQuery request, CancellationToken ct)
        {
            var category = await _categoryReadOnlyRepository.GetByIdAsync(request.CategoryId, ct);

            if (category == null)
                throw new NotFoundException("Category not found.");

            return new GetCategoryResponse()
            {
                Name = category.Name,
                Products = category.Products.Select(p => new GetCategoryProductResponse
                {
                    Sku = p.Sku,
                    Barcode = p.Barcode,
                    Name = p.Name,
                    Description = p.Description,
                    CostPrice = p.CostPrice,
                    SalePrice = p.SalePrice,
                    CurrentStock = p.CurrentStock,
                    ExpirationDate = p.ExpirationDate
                }).ToList()
            };
        }
    }
}