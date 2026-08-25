using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Product.Commands;

public class DeactivateProduct
{
    public sealed record DeactivateProductCommand(Guid Id) : IRequest;

    public class DeactivateProductCommandHandler : IRequestHandler<DeactivateProductCommand>
    {
        private readonly IProductWriteOnlyRepository _productWriteOnlyRepository;
        private readonly IProductReadOnlyRepository _productReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateProductCommandHandler(
            IProductWriteOnlyRepository productWriteOnlyRepository,
            IProductReadOnlyRepository productReadOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _productWriteOnlyRepository = productWriteOnlyRepository;
            _productReadOnlyRepository = productReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeactivateProductCommand command, CancellationToken ct)
        {
            var product = await _productReadOnlyRepository.GetByIdAsync(command.Id, ct);

            if (product is null)
                throw new NotFoundException($"Product with id '{command.Id}' not found.");

            product.Deactivate();

            await _productWriteOnlyRepository.UpdateAsync(product, ct);

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}