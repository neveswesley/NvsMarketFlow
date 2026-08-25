using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Supplier.Commands;

public class ActivateSupplier
{
    public sealed record ActivateSupplierCommand(Guid Id) : IRequest;

    public class ActivateSupplierCommandHandler : IRequestHandler<ActivateSupplierCommand>
    {
        private readonly ISupplierWriteOnlyRepository _supplierWriteOnlyRepository;
        private readonly ISupplierReadOnlyRepository _supplierReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ActivateSupplierCommandHandler(
            ISupplierWriteOnlyRepository supplierWriteOnlyRepository,
            ISupplierReadOnlyRepository supplierReadOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _supplierWriteOnlyRepository = supplierWriteOnlyRepository;
            _supplierReadOnlyRepository = supplierReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ActivateSupplierCommand command, CancellationToken ct)
        {
            var supplier = await _supplierReadOnlyRepository.GetByIdAsync(command.Id, ct);

            if (supplier is null)
                throw new NotFoundException($"Supplier with id '{command.Id}' not found.");

            supplier.Activate();

            await _supplierWriteOnlyRepository.UpdateAsync(supplier, ct);

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}