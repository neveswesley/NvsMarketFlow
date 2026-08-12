using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Responses.Product;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;

namespace NvsMarketFlow.Application.UseCases.Product.Queries;

public class GetProductById
{
    public sealed record GetProductByIdQuery(Guid ProductId) : IRequest<GetProductResponse>;

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, GetProductResponse>
    {

        private readonly IProductReadOnlyRepository _productReadOnlyRepository;

        public GetProductByIdQueryHandler(IProductReadOnlyRepository productReadOnlyRepository)
        {
            _productReadOnlyRepository = productReadOnlyRepository;
        }

        public async Task<GetProductResponse> Handle(GetProductByIdQuery request, CancellationToken ct)
        {
            var product = await _productReadOnlyRepository.GetByIdAsync(request.ProductId, ct);

            if (product == null)
                throw new NotFoundException("Product not found.");

            return new GetProductResponse()
            {
                Id = product.Id,
                Sku = product.Sku,
                Barcode = product.Barcode,
                Name = product.Name,
                CategoryName = product.Category.Name,
                BrandName = product.Brand?.Name,
                SalePrice = product.SalePrice,
                CurrentStock = product.CurrentStock,
                MinimumStock = product.MinimumStock,
                Unit = product.Unit,
                Status = product.Status,
                ExpirationDate = product.ExpirationDate
            };

        }
    }
}