using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Purchase.Commands;

public class CancelPurchase
{
    public sealed record CancelPurchaseCommand(Guid Id) : IRequest;

    public class CancelPurchaseCommandHandler : IRequestHandler<CancelPurchaseCommand>
    {
        private readonly IPurchaseReadOnlyRepository _purchaseReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CancelPurchaseCommandHandler(
            IPurchaseReadOnlyRepository purchaseReadOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _purchaseReadOnlyRepository = purchaseReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CancelPurchaseCommand command, CancellationToken ct)
        {
            var purchase = await _purchaseReadOnlyRepository.GetByIdAsync(command.Id, ct);

            if (purchase is null)
                throw new NotFoundException($"Purchase with id '{command.Id}' not found.");

            purchase.Cancel();

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}