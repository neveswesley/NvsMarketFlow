using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Responses.Sale;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;

namespace NvsMarketFlow.Application.UseCases.Sale.Queries;

public class GetSaleById
{
    public sealed record GetSaleByIdQuery(Guid Id) : IRequest<GetSaleByIdResponse>;

    public class GetSaleByIdQueryHandler : IRequestHandler<GetSaleByIdQuery, GetSaleByIdResponse>
    {
        private readonly ISaleReadOnlyRepository _saleReadOnlyRepository;

        public GetSaleByIdQueryHandler(ISaleReadOnlyRepository saleReadOnlyRepository)
        {
            _saleReadOnlyRepository = saleReadOnlyRepository;
        }

        public async Task<GetSaleByIdResponse> Handle(GetSaleByIdQuery request, CancellationToken ct)
        {
            var sale = await _saleReadOnlyRepository.GetByIdAsync(request.Id, ct);

            if (sale is null)
                throw new NotFoundException($"Sale with id '{request.Id}' not found.");

            return new GetSaleByIdResponse
            {
                Id = sale.Id,
                CashRegisterId = sale.CashRegisterId,
                SellerId = sale.SellerId,
                SellerName = sale.Seller.Name,
                SaleNumber = sale.SaleNumber,
                Subtotal = sale.Subtotal,
                Discount = sale.Discount,
                Total = sale.Total,
                Status = sale.Status,
                Items = sale.Items
                    .Select(i => new GetSaleItemResponse
                    {
                        Id = i.Id,
                        ProductId = i.ProductId,
                        ProductName = i.Product.Name,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        Discount = i.Discount,
                        Total = i.Total
                    })
                    .ToList(),
                Payments = sale.Payments
                    .Select(p => new GetPaymentResponse
                    {
                        Id = p.Id,
                        Method = p.Method,
                        Value = p.Value
                    })
                    .ToList(),
                CreatedAt = sale.CreatedAt
            };
        }
    }
}