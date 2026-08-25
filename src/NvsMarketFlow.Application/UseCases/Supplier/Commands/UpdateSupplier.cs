using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Requests.Supplier;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Supplier.Commands;

public class UpdateSupplier
{
    public sealed record UpdateSupplierCommand(Guid Id, UpdateSupplierInfoRequest Request) : IRequest;

    public class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand>
    {
        private readonly ISupplierWriteOnlyRepository _supplierWriteOnlyRepository;
        private readonly ISupplierReadOnlyRepository _supplierReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSupplierCommandHandler(
            ISupplierWriteOnlyRepository supplierWriteOnlyRepository,
            ISupplierReadOnlyRepository supplierReadOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _supplierWriteOnlyRepository = supplierWriteOnlyRepository;
            _supplierReadOnlyRepository = supplierReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateSupplierCommand command, CancellationToken ct)
        {
            var supplier = await _supplierReadOnlyRepository.GetByIdAsync(command.Id, ct);

            if (supplier is null)
                throw new NotFoundException($"Supplier with id '{command.Id}' not found.");

            var existingName = await _supplierReadOnlyRepository
                .ExistsByCorporateNameAsync(command.Request.CorporateName, command.Id, ct);

            if (existingName)
                throw new DuplicateFieldException("Supplier", "corporate name", command.Request.CorporateName);

            var existingCnpj = await _supplierReadOnlyRepository
                .ExistsByCnpjAsync(command.Request.CNPJ, command.Id, ct);

            if (existingCnpj)
                throw new DuplicateFieldException("Supplier", "CNPJ", command.Request.CNPJ);

            supplier.UpdateInfo(
                command.Request.CorporateName,
                command.Request.FantasyName,
                command.Request.CNPJ,
                command.Request.Phone,
                command.Request.Email,
                command.Request.Address);

            await _supplierWriteOnlyRepository.UpdateAsync(supplier, ct);

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}