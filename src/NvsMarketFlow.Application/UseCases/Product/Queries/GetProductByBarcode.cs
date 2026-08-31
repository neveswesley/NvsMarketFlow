using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Responses.Product;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;

namespace NvsMarketFlow.Application.UseCases.Product.Queries;

public class GetProductByBarcode
{
    public sealed record GetProductByBarcodeQuery(string Barcode) : IRequest<GetProductByBarcodeResponse>;

    public class GetProductByBarcodeQueryHandler : IRequestHandler<GetProductByBarcodeQuery, GetProductByBarcodeResponse>
    {
        private readonly IProductReadOnlyRepository _productReadOnlyRepository;

        public GetProductByBarcodeQueryHandler(IProductReadOnlyRepository productReadOnlyRepository)
        {
            _productReadOnlyRepository = productReadOnlyRepository;
        }

        public async Task<GetProductByBarcodeResponse> Handle(GetProductByBarcodeQuery request, CancellationToken ct)
        {
            var product = await _productReadOnlyRepository.GetByBarcodeAsync(request.Barcode, ct);

            if (product is null)
                throw new NotFoundException($"Product with barcode '{request.Barcode}' not found.");

            return new GetProductByBarcodeResponse
            {
                Id = product.Id,
                Sku = product.Sku,
                Barcode = product.Barcode,
                Name = product.Name,
                CategoryName = product.Category.Name,
                BrandName = product.Brand?.Name,
                SupplierName = product.Supplier?.FantasyName,
                SalePrice = product.SalePrice,
                CurrentStock = product.CurrentStock,
                MinimumStock = product.MinimumStock,
                Unit = product.Unit,
                Status = product.Status
            };
        }
    }
}