using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Product.Commands;

public class DeleteProduct
{
    public sealed record DeleteProductCommand (Guid Id) : IRequest<Unit>;

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
    {
        
        private readonly IProductWriteOnlyRepository _productWriteOnlyRepository;
        private readonly IProductReadOnlyRepository _productReadOnlyRepository;

        public DeleteProductCommandHandler(IProductWriteOnlyRepository productWriteOnlyRepository, IProductReadOnlyRepository productReadOnlyRepository)
        {
            _productWriteOnlyRepository = productWriteOnlyRepository;
            _productReadOnlyRepository = productReadOnlyRepository;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productReadOnlyRepository.GetByIdAsync(request.Id, cancellationToken);
            if (product == null)
                throw new NotFoundException("Product not found.");
            
            product.Deactivate();
            
            await _productWriteOnlyRepository.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}