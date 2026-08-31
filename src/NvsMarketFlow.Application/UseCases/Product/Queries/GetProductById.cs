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
                Description = product.Description,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name,
                BrandId = product.BrandId,
                BrandName = product.Brand?.Name,
                SupplierId = product.SupplierId,
                CostPrice = product.CostPrice,
                SalePrice = product.SalePrice,
                CurrentStock = product.CurrentStock,
                MinimumStock = product.MinimumStock,
                MaximumStock = product.MaximumStock,
                Unit = product.Unit,
                Status = product.Status,
                ExpirationDate = product.ExpirationDate
            };

        }
    }
}