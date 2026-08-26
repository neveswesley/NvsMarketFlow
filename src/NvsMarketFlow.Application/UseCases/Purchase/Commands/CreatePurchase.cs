using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Requests.Purchase;
using NvsMarketFlow.Application.Responses.Purchase;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Purchase.Commands;

public class CreatePurchase
{
    public sealed record CreatePurchaseCommand(CreatePurchaseRequest Request) : IRequest<CreatePurchaseResponse>;

    public class CreatePurchaseCommandHandler : IRequestHandler<CreatePurchaseCommand, CreatePurchaseResponse>
    {
        private readonly IPurchaseWriteOnlyRepository _purchaseWriteOnlyRepository;
        private readonly IPurchaseReadOnlyRepository _purchaseReadOnlyRepository;
        private readonly ISupplierReadOnlyRepository _supplierReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePurchaseCommandHandler(
            IPurchaseWriteOnlyRepository purchaseWriteOnlyRepository,
            IPurchaseReadOnlyRepository purchaseReadOnlyRepository,
            ISupplierReadOnlyRepository supplierReadOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _purchaseWriteOnlyRepository = purchaseWriteOnlyRepository;
            _purchaseReadOnlyRepository = purchaseReadOnlyRepository;
            _supplierReadOnlyRepository = supplierReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreatePurchaseResponse> Handle(CreatePurchaseCommand command, CancellationToken ct)
        {
            var supplier = await _supplierReadOnlyRepository.GetByIdAsync(command.Request.SupplierId, ct);

            if (supplier is null)
                throw new NotFoundException($"Supplier with id '{command.Request.SupplierId}' not found.");

            var existingInvoice = await _purchaseReadOnlyRepository
                .ExistsByInvoiceNumberAsync(command.Request.InvoiceNumber, null, ct);

            if (existingInvoice)
                throw new DuplicateFieldException("Purchase", "invoice number", command.Request.InvoiceNumber);

            var purchase = new Domain.Entities.Purchase(
                command.Request.SupplierId,
                command.Request.InvoiceNumber);

            await _purchaseWriteOnlyRepository.CreateAsync(purchase, ct);

            await _unitOfWork.SaveChangesAsync(ct);

            return new CreatePurchaseResponse
            {
                Id = purchase.Id,
                SupplierId = purchase.SupplierId,
                InvoiceNumber = purchase.InvoiceNumber,
                Total = purchase.Total,
                Status = purchase.Status
            };
        }
    }
}