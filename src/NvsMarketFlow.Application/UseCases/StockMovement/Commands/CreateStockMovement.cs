using MediatR;
using NvsMarketFlow.Application.Requests.StockMovement;
using NvsMarketFlow.Application.Responses.StockMovement;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.StockMovement.Commands;

public class CreateStockMovement
{
    public sealed record CreateStockMovementCommand(CreateStockMovementRequest Request)
        : IRequest<CreateStockMovementResponse>;
 
    public class
        CreateStockMovementCommandHandler : IRequestHandler<CreateStockMovementCommand, CreateStockMovementResponse>
    {
        private readonly IStockMovementWriteOnlyRepository _stockMovementWriteOnlyRepository;
        private readonly IProductReadOnlyRepository _productReadOnlyRepository;
        private readonly IProductWriteOnlyRepository _productWriteOnlyRepository;
 
        public CreateStockMovementCommandHandler(
            IStockMovementWriteOnlyRepository stockMovementWriteOnlyRepository,
            IProductReadOnlyRepository productReadOnlyRepository,
            IProductWriteOnlyRepository productWriteOnlyRepository)
        {
            _stockMovementWriteOnlyRepository = stockMovementWriteOnlyRepository;
            _productReadOnlyRepository = productReadOnlyRepository;
            _productWriteOnlyRepository = productWriteOnlyRepository;
        }
 
        public async Task<CreateStockMovementResponse> Handle(CreateStockMovementCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _productReadOnlyRepository.GetByIdAsync(request.Request.ProductId, cancellationToken)
                ?? throw new InvalidOperationException("Product not found.");
 
            product.ApplyStockMovement(
                request.Request.MovementType,
                request.Request.Quantity,
                request.Request.IsIncrease);
 
            var stockMovement = new Domain.Entities.StockMovement(request.Request.ProductId, request.Request.UserId,
                request.Request.MovementType, request.Request.Quantity, request.Request.Reason);
 
            await _stockMovementWriteOnlyRepository.CreateAsync(stockMovement, cancellationToken);
 
            return new CreateStockMovementResponse()
            {
                Id = stockMovement.Id,
                ProductId = stockMovement.ProductId,
                UserId = stockMovement.UserId,
                MovementType = stockMovement.MovementType,
                Quantity = stockMovement.Quantity,
                Reason = stockMovement.Reason
            };
        }
    }
}
 