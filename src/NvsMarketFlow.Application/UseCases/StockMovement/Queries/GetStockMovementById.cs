using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Responses.StockMovement;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;

namespace NvsMarketFlow.Application.UseCases.StockMovement.Queries;

public class GetStockMovementById
{
    public sealed record GetStockMovementByIdQuery(Guid Id) : IRequest<GetStockMovementByIdResponse>;

    public class GetStockMovementByIdQueryHandler : IRequestHandler<GetStockMovementByIdQuery, GetStockMovementByIdResponse>
    {
        private readonly IStockMovementReadOnlyRepository _stockMovementReadOnlyRepository;

        public GetStockMovementByIdQueryHandler(IStockMovementReadOnlyRepository stockMovementReadOnlyRepository)
        {
            _stockMovementReadOnlyRepository = stockMovementReadOnlyRepository;
        }

        public async Task<GetStockMovementByIdResponse> Handle(GetStockMovementByIdQuery request, CancellationToken ct)
        {
            var stockMovement = await _stockMovementReadOnlyRepository.GetByIdAsync(request.Id, ct);

            if (stockMovement is null)
                throw new NotFoundException($"StockMovement with id '{request.Id}' not found.");

            return new GetStockMovementByIdResponse
            {
                Id = stockMovement.Id,
                ProductId = stockMovement.ProductId,
                ProductName = stockMovement.Product.Name,
                MovementType = stockMovement.MovementType,
                Quantity = stockMovement.Quantity,
                Reason = stockMovement.Reason,
                UserId = stockMovement.UserId,
                UserName = stockMovement.User.Name,
                Date = stockMovement.Date
            };
        }
    }
}