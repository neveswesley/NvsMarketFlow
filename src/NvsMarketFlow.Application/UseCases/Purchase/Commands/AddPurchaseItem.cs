using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Requests.Purchase;
using NvsMarketFlow.Application.Responses.Purchase;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Purchase.Commands;

public class AddPurchaseItem
{
    public sealed record AddPurchaseItemCommand(Guid PurchaseId, AddPurchaseItemRequest Request)
        : IRequest<AddPurchaseItemResponse>;

    public class AddPurchaseItemCommandHandler : IRequestHandler<AddPurchaseItemCommand, AddPurchaseItemResponse>
    {
        private readonly IPurchaseWriteOnlyRepository _purchaseWriteOnlyRepository;
        private readonly IPurchaseReadOnlyRepository _purchaseReadOnlyRepository;
        private readonly IProductReadOnlyRepository _productReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddPurchaseItemCommandHandler(
            IPurchaseWriteOnlyRepository purchaseWriteOnlyRepository,
            IPurchaseReadOnlyRepository purchaseReadOnlyRepository,
            IProductReadOnlyRepository productReadOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _purchaseWriteOnlyRepository = purchaseWriteOnlyRepository;
            _purchaseReadOnlyRepository = purchaseReadOnlyRepository;
            _productReadOnlyRepository = productReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AddPurchaseItemResponse> Handle(AddPurchaseItemCommand command, CancellationToken ct)
        {
            var purchase = await _purchaseReadOnlyRepository.GetByIdAsync(command.PurchaseId, ct);

            if (purchase is null)
                throw new NotFoundException($"Purchase with id '{command.PurchaseId}' not found.");

            var product = await _productReadOnlyRepository.GetByIdAsync(command.Request.ProductId, ct);

            if (product is null)
                throw new NotFoundException($"Product with id '{command.Request.ProductId}' not found.");

            var item = purchase.AddItem(
                command.Request.ProductId,
                command.Request.Quantity,
                command.Request.CostPrice);

            await _purchaseWriteOnlyRepository.AddItemAsync(item, ct);

            await _unitOfWork.SaveChangesAsync(ct);

            return new AddPurchaseItemResponse
            {
                PurchaseId = purchase.Id,
                ProductId = command.Request.ProductId,
                Quantity = command.Request.Quantity,
                CostPrice = command.Request.CostPrice,
                PurchaseTotal = purchase.Total
            };
        }
    }
}