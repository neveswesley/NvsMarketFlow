using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Requests.Purchase;
using NvsMarketFlow.Application.Responses.Purchase;
using NvsMarketFlow.Domain.Enums;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Purchase.Commands;

public class ConfirmPurchase
{
    public sealed record ConfirmPurchaseCommand(Guid Id, ConfirmPurchaseRequest Request)
        : IRequest<ConfirmPurchaseResponse>;

    public class ConfirmPurchaseCommandHandler : IRequestHandler<ConfirmPurchaseCommand, ConfirmPurchaseResponse>
    {
        private readonly IPurchaseReadOnlyRepository _purchaseReadOnlyRepository;
        private readonly IStockMovementWriteOnlyRepository _stockMovementWriteOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmPurchaseCommandHandler(
            IPurchaseReadOnlyRepository purchaseReadOnlyRepository,
            IStockMovementWriteOnlyRepository stockMovementWriteOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _purchaseReadOnlyRepository = purchaseReadOnlyRepository;
            _stockMovementWriteOnlyRepository = stockMovementWriteOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ConfirmPurchaseResponse> Handle(ConfirmPurchaseCommand command, CancellationToken ct)
        {
            var purchase = await _purchaseReadOnlyRepository.GetByIdAsync(command.Id, ct);

            if (purchase is null)
                throw new NotFoundException($"Purchase with id '{command.Id}' not found.");

            purchase.Confirm();

            var warnings = new List<string>();

            foreach (var item in purchase.Items)
            {
                item.Product.ApplyStockMovement(MovementType.In, item.Quantity, bypassMaximumStock: true);
                item.Product.UpdateCostPrice(item.CostPrice);

                if (item.Product.ExceedsMaximumStock)
                {
                    warnings.Add(
                        $"Product '{item.Product.Name}' stock ({item.Product.CurrentStock}) now exceeds its maximum stock ({item.Product.MaximumStock}).");
                }

                var stockMovement = new Domain.Entities.StockMovement(
                    item.ProductId,
                    command.Request.UserId,
                    MovementType.In,
                    item.Quantity,
                    $"Purchase confirmed - invoice {purchase.InvoiceNumber}");

                await _stockMovementWriteOnlyRepository.CreateAsync(stockMovement, ct);
            }

            await _unitOfWork.SaveChangesAsync(ct);

            return new ConfirmPurchaseResponse
            {
                Id = purchase.Id,
                InvoiceNumber = purchase.InvoiceNumber,
                Total = purchase.Total,
                Status = purchase.Status,
                Warnings = warnings
            };
        }
    }
}