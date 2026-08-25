using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Supplier.Commands;

public class DeactivateSupplier
{
    public sealed record DeactivateSupplierCommand(Guid Id) : IRequest;

    public class DeactivateSupplierCommandHandler : IRequestHandler<DeactivateSupplierCommand>
    {
        private readonly ISupplierWriteOnlyRepository _supplierWriteOnlyRepository;
        private readonly ISupplierReadOnlyRepository _supplierReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateSupplierCommandHandler(
            ISupplierWriteOnlyRepository supplierWriteOnlyRepository,
            ISupplierReadOnlyRepository supplierReadOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _supplierWriteOnlyRepository = supplierWriteOnlyRepository;
            _supplierReadOnlyRepository = supplierReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeactivateSupplierCommand command, CancellationToken ct)
        {
            var supplier = await _supplierReadOnlyRepository.GetByIdAsync(command.Id, ct);

            if (supplier is null)
                throw new NotFoundException($"Supplier with id '{command.Id}' not found.");

            supplier.Deactivate();

            await _supplierWriteOnlyRepository.UpdateAsync(supplier, ct);

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}