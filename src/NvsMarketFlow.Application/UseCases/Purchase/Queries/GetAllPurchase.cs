using MediatR;
using NvsMarketFlow.Application.Common;
using NvsMarketFlow.Application.Responses.Purchase;
using NvsMarketFlow.Domain.Enums;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;

namespace NvsMarketFlow.Application.UseCases.Purchase.Queries;

public class GetAllPurchase
{
    public sealed record GetAllPurchaseQuery(
        Guid? SupplierId,
        string? InvoiceNumber,
        PurchaseStatus? Status,
        DateTime? StartDate,
        DateTime? EndDate,
        int Page = 1,
        int PageSize = 10
    ) : IRequest<PagedResult<GetPurchaseResponse>>;

    public class GetAllPurchaseQueryHandler : IRequestHandler<GetAllPurchaseQuery, PagedResult<GetPurchaseResponse>>
    {
        private readonly IPurchaseReadOnlyRepository _purchaseReadOnlyRepository;

        public GetAllPurchaseQueryHandler(IPurchaseReadOnlyRepository purchaseReadOnlyRepository)
        {
            _purchaseReadOnlyRepository = purchaseReadOnlyRepository;
        }

        public async Task<PagedResult<GetPurchaseResponse>> Handle(GetAllPurchaseQuery request, CancellationToken ct)
        {
            var result = await _purchaseReadOnlyRepository.GetAllAsync(
                request.SupplierId,
                request.InvoiceNumber,
                request.Status,
                request.StartDate,
                request.EndDate,
                request.Page,
                request.PageSize,
                ct);

            var items = result.Items
                .Select(p => new GetPurchaseResponse
                {
                    Id = p.Id,
                    SupplierId = p.SupplierId,
                    SupplierName = p.Supplier.FantasyName,
                    InvoiceNumber = p.InvoiceNumber,
                    Total = p.Total,
                    Status = p.Status,
                    CreatedAt = p.CreatedAt
                })
                .ToList();

            return new PagedResult<GetPurchaseResponse>(
                items,
                result.Page,
                result.PageSize,
                result.TotalItems,
                result.TotalPages);
        }
    }
}