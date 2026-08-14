using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Product.Commands;

public class ActivateProduct
{
    public sealed record ActivateProductCommand(Guid Id) : IRequest<Unit>;

    public class ActivateProductCommandHandler : IRequestHandler<ActivateProductCommand, Unit>
    {
        
        private readonly IProductWriteOnlyRepository _productWriteOnlyRepository;
        private readonly IProductReadOnlyRepository _productReadOnlyRepository;

        public ActivateProductCommandHandler(IProductWriteOnlyRepository productWriteOnlyRepository, IProductReadOnlyRepository productReadOnlyRepository)
        {
            _productWriteOnlyRepository = productWriteOnlyRepository;
            _productReadOnlyRepository = productReadOnlyRepository;
        }

        public async Task<Unit> Handle(ActivateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productReadOnlyRepository.GetByIdAsync(request.Id, cancellationToken);
            if (product == null)
                throw new NotFoundException("Product not found.");
            
            product.Activate();
            await _productWriteOnlyRepository.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}