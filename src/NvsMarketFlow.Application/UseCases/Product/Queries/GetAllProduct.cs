using MediatR;
using NvsMarketFlow.Application.Responses.Product;
using NvsMarketFlow.Domain.Common;
using NvsMarketFlow.Domain.Enums;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;

namespace NvsMarketFlow.Application.UseCases.Product.Queries;

public class GetAllProduct
{
    public sealed record GetAllProductQuery(
        string? Name,
        Guid? CategoryId,
        Guid? BrandId,
        Status? Status,
        decimal? MinPrice,
        decimal? MaxPrice,
        bool? LowStock,
        int Page = 1,
        int PageSize = 10
    ) : IRequest<PagedResult<GetProductResponse>>;
    
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, PagedResult<GetProductResponse>>
    {

        private readonly IProductReadOnlyRepository _productReadOnlyRepository;

        public GetAllProductQueryHandler(IProductReadOnlyRepository productReadOnlyRepository)
        {
            _productReadOnlyRepository = productReadOnlyRepository;
        }

        public async Task<PagedResult<GetProductResponse>> Handle(GetAllProductQuery request, CancellationToken ct)
        {
            var result = await _productReadOnlyRepository.GetAllAsync(
                request.Name,
                request.CategoryId,
                request.BrandId,
                request.Status,
                request.MinPrice,
                request.MaxPrice,
                request.LowStock,
                request.Page,
                request.PageSize,
                ct);

            var items = result.Items
                .Select(c => new GetProductResponse
                {
                    Id = c.Id,
                    Sku = c.Sku,
                    Barcode = c.Barcode,
                    Name = c.Name,
                    Description = c.Description,
                    CategoryId = c.CategoryId,
                    CategoryName = c.Category.Name,
                    BrandId = c.BrandId,
                    BrandName = c.Brand?.Name,
                    SupplierId = c.SupplierId,
                    CostPrice = c.CostPrice,
                    SalePrice = c.SalePrice,
                    CurrentStock = c.CurrentStock,
                    MinimumStock = c.MinimumStock,
                    MaximumStock = c.MaximumStock,
                    Unit = c.Unit,
                    Status = c.Status,
                    ExpirationDate = c.ExpirationDate
                })
                .ToList();
            
            return new PagedResult<GetProductResponse>(
                items,
                result.Page,
                result.PageSize,
                result.TotalItems,
                result.TotalPages);
        }
    }
}