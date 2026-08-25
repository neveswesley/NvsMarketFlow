using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Requests.Supplier;
using NvsMarketFlow.Application.Responses.Supplier;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Supplier.Commands;

public class CreateSupplier
{
    public sealed record CreateSupplierCommand(CreateSupplierRequest Request) : IRequest<CreateSupplierResponse>;

    public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, CreateSupplierResponse>
    {
        private readonly ISupplierWriteOnlyRepository _supplierWriteOnlyRepository;
        private readonly ISupplierReadOnlyRepository _supplierReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSupplierCommandHandler(
            ISupplierWriteOnlyRepository supplierWriteOnlyRepository,
            ISupplierReadOnlyRepository supplierReadOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _supplierWriteOnlyRepository = supplierWriteOnlyRepository;
            _supplierReadOnlyRepository = supplierReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateSupplierResponse> Handle(CreateSupplierCommand command, CancellationToken ct)
        {
            var existingName = await _supplierReadOnlyRepository
                .ExistsByCorporateNameAsync(command.Request.CorporateName, null, ct);

            if (existingName)
                throw new DuplicateFieldException("Supplier", "corporate name", command.Request.CorporateName);

            var existingCnpj = await _supplierReadOnlyRepository
                .ExistsByCnpjAsync(command.Request.CNPJ, null, ct);

            if (existingCnpj)
                throw new DuplicateFieldException("Supplier", "CNPJ", command.Request.CNPJ);

            if (existingCnpj)
                throw new DuplicateFieldException("Supplier", "CNPJ", command.Request.CNPJ);

            var supplier = new Domain.Entities.Supplier(
                command.Request.CorporateName,
                command.Request.FantasyName,
                command.Request.CNPJ,
                command.Request.Phone,
                command.Request.Email,
                command.Request.Address,
                command.Request.Status);

            await _supplierWriteOnlyRepository.CreateAsync(supplier, ct);

            await _unitOfWork.SaveChangesAsync(ct);

            return new CreateSupplierResponse
            {
                Id = supplier.Id,
                CorporateName = supplier.CorporateName,
                FantasyName = supplier.FantasyName,
                CNPJ = supplier.CNPJ
            };
        }
    }
}