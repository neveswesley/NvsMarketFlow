using MediatR;
using NvsMarketFlow.Application.Responses.Sale;
using NvsMarketFlow.Domain.Common;
using NvsMarketFlow.Domain.Enums;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;

namespace NvsMarketFlow.Application.UseCases.Sale.Queries;

public class GetAllSale
{
    public sealed record GetAllSaleQuery(
        Guid? CashRegisterId,
        Guid? SellerId,
        string? SaleNumber,
        SaleStatus? Status,
        DateTime? StartDate,
        DateTime? EndDate,
        int Page = 1,
        int PageSize = 10
    ) : IRequest<PagedResult<GetSaleResponse>>;

    public class GetAllSaleQueryHandler : IRequestHandler<GetAllSaleQuery, PagedResult<GetSaleResponse>>
    {
        private readonly ISaleReadOnlyRepository _saleReadOnlyRepository;

        public GetAllSaleQueryHandler(ISaleReadOnlyRepository saleReadOnlyRepository)
        {
            _saleReadOnlyRepository = saleReadOnlyRepository;
        }

        public async Task<PagedResult<GetSaleResponse>> Handle(GetAllSaleQuery request, CancellationToken ct)
        {
            var result = await _saleReadOnlyRepository.GetAllAsync(
                request.CashRegisterId,
                request.SellerId,
                request.SaleNumber,
                request.Status,
                request.StartDate,
                request.EndDate,
                request.Page,
                request.PageSize,
                ct);

            var items = result.Items
                .Select(s => new GetSaleResponse
                {
                    Id = s.Id,
                    SellerId = s.SellerId,
                    SellerName = s.Seller.Name,
                    SaleNumber = s.SaleNumber,
                    Total = s.Total,
                    Status = s.Status,
                    CreatedAt = s.CreatedAt
                })
                .ToList();

            return new PagedResult<GetSaleResponse>(
                items,
                result.Page,
                result.PageSize,
                result.TotalItems,
                result.TotalPages);
        }
    }
}