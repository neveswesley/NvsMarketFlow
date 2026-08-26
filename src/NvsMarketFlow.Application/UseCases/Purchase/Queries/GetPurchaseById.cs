using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Responses.Purchase;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;

namespace NvsMarketFlow.Application.UseCases.Purchase.Queries;

public class GetPurchaseById
{
    public sealed record GetPurchaseByIdQuery(Guid Id) : IRequest<GetPurchaseByIdResponse>;

    public class GetPurchaseByIdQueryHandler : IRequestHandler<GetPurchaseByIdQuery, GetPurchaseByIdResponse>
    {
        private readonly IPurchaseReadOnlyRepository _purchaseReadOnlyRepository;

        public GetPurchaseByIdQueryHandler(IPurchaseReadOnlyRepository purchaseReadOnlyRepository)
        {
            _purchaseReadOnlyRepository = purchaseReadOnlyRepository;
        }

        public async Task<GetPurchaseByIdResponse> Handle(GetPurchaseByIdQuery request, CancellationToken ct)
        {
            var purchase = await _purchaseReadOnlyRepository.GetByIdAsync(request.Id, ct);

            if (purchase is null)
                throw new NotFoundException($"Purchase with id '{request.Id}' not found.");

            return new GetPurchaseByIdResponse
            {
                Id = purchase.Id,
                SupplierId = purchase.SupplierId,
                SupplierName = purchase.Supplier.FantasyName,
                InvoiceNumber = purchase.InvoiceNumber,
                Total = purchase.Total,
                Status = purchase.Status,
                Items = purchase.Items
                    .Select(i => new GetPurchaseItemResponse
                    {
                        Id = i.Id,
                        ProductId = i.ProductId,
                        ProductName = i.Product.Name,
                        Quantity = i.Quantity,
                        CostPrice = i.CostPrice
                    })
                    .ToList(),
                CreatedAt = purchase.CreatedAt
            };
        }
    }
}