using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Responses.Supplier;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;

namespace NvsMarketFlow.Application.UseCases.Supplier.Queries;

public class GetSupplierById
{
    public sealed record GetSupplierByIdQuery(Guid Id) : IRequest<GetSupplierByIdResponse>;

    public class GetSupplierByIdQueryHandler : IRequestHandler<GetSupplierByIdQuery, GetSupplierByIdResponse>
    {
        private readonly ISupplierReadOnlyRepository _supplierReadOnlyRepository;

        public GetSupplierByIdQueryHandler(ISupplierReadOnlyRepository supplierReadOnlyRepository)
        {
            _supplierReadOnlyRepository = supplierReadOnlyRepository;
        }

        public async Task<GetSupplierByIdResponse> Handle(GetSupplierByIdQuery request, CancellationToken ct)
        {
            var supplier = await _supplierReadOnlyRepository.GetByIdAsync(request.Id, ct);

            if (supplier is null)
                throw new NotFoundException($"Supplier with id '{request.Id}' not found.");

            return new GetSupplierByIdResponse
            {
                Id = supplier.Id,
                CorporateName = supplier.CorporateName,
                FantasyName = supplier.FantasyName,
                CNPJ = supplier.CNPJ,
                Phone = supplier.Phone,
                Email = supplier.Email,
                Address = supplier.Address,
                Status = supplier.Status,
                CreatedAt = supplier.CreatedAt,
                UpdatedAt = supplier.UpdatedAt
            };
        }
    }
}