using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Requests.Product;
using NvsMarketFlow.Application.Responses.Product;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Product.Commands;

public class CreateProduct
{
    public sealed record CreateProductCommand(CreateProductRequest Request, CancellationToken ct) : IRequest<CreateProductResponse>;

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductResponse>
    {
        private readonly IProductWriteOnlyRepository _productWriteOnlyRepository;
        private readonly IProductReadOnlyRepository _productReadOnlyRepository;

        public CreateProductCommandHandler(IProductWriteOnlyRepository productWriteOnlyRepository,
            IProductReadOnlyRepository productReadOnlyRepository)
        {
            _productWriteOnlyRepository = productWriteOnlyRepository;
            _productReadOnlyRepository = productReadOnlyRepository;
        }

        public async Task<CreateProductResponse> Handle(CreateProductCommand command, CancellationToken ct)
        {
            
            var existingName = await _productReadOnlyRepository.ExistsByNameAsync(command.Request.Name, ct);
            
            if (existingName)
                throw new DuplicateFieldException
                    ("Product", "name", command.Request.Name);
            
            var existingSku = await _productReadOnlyRepository
                .ExistsBySkuAsync(command.Request.Sku, ct);

            if (existingSku)
                throw new DuplicateFieldException(
                    "Product", "Sku", command.Request.Sku);
            
            var product = new Domain.Entities.Product(
                command.Request.Sku,
                command.Request.Name,
                command.Request.Description,
                command.Request.CategoryId,
                command.Request.BrandId,
                command.Request.CostPrice,
                command.Request.SalePrice,
                command.Request.CurrentStock,
                command.Request.MinimumStock,
                command.Request.MaximumStock,
                command.Request.ExpirationDate,
                command.Request.Unit,
                command.Request.Status);

            await _productWriteOnlyRepository.CreateAsync(product, ct);

            return new CreateProductResponse()
            {
                Id = product.Id,
                Name = product.Name,
                SalePrice = product.SalePrice,
                CategoryId = product.CategoryId
            };
        }
    }
}