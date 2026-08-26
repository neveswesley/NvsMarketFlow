using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Purchase.Commands;

public class RemovePurchaseItem
{
    public sealed record RemovePurchaseItemCommand(Guid PurchaseId, Guid ItemId) : IRequest;

    public class RemovePurchaseItemCommandHandler : IRequestHandler<RemovePurchaseItemCommand>
    {
        private readonly IPurchaseWriteOnlyRepository _purchaseWriteOnlyRepository;
        private readonly IPurchaseReadOnlyRepository _purchaseReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemovePurchaseItemCommandHandler(
            IPurchaseWriteOnlyRepository purchaseWriteOnlyRepository,
            IPurchaseReadOnlyRepository purchaseReadOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _purchaseWriteOnlyRepository = purchaseWriteOnlyRepository;
            _purchaseReadOnlyRepository = purchaseReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(RemovePurchaseItemCommand command, CancellationToken ct)
        {
            var purchase = await _purchaseReadOnlyRepository.GetByIdAsync(command.PurchaseId, ct);

            if (purchase is null)
                throw new NotFoundException($"Purchase with id '{command.PurchaseId}' not found.");

            purchase.RemoveItem(command.ItemId);

            await _purchaseWriteOnlyRepository.UpdateAsync(purchase, ct);

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}