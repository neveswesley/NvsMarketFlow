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
        
        private readonly IProductReadOnlyRepository _productReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(IProductReadOnlyRepository productReadOnlyRepository, IUnitOfWork unitOfWork)
        {
            _productReadOnlyRepository = productReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productReadOnlyRepository.GetByIdAsync(request.Id, cancellationToken);
            if (product == null)
                throw new NotFoundException("Product not found.");
            
            product.Deactivate();
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}