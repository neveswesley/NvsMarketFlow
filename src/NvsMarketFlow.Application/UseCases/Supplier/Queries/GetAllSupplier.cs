using MediatR;
using NvsMarketFlow.Application.Common;
using NvsMarketFlow.Application.Responses.Supplier;
using NvsMarketFlow.Domain.Enums;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;

namespace NvsMarketFlow.Application.UseCases.Supplier.Queries;

public class GetAllSupplier
{
    public sealed record GetAllSupplierQuery(
        string? CorporateName,
        string? FantasyName,
        string? CNPJ,
        Status? Status,
        int Page = 1,
        int PageSize = 10
    ) : IRequest<PagedResult<GetSupplierResponse>>;

    public class GetAllSupplierQueryHandler : IRequestHandler<GetAllSupplierQuery, PagedResult<GetSupplierResponse>>
    {
        private readonly ISupplierReadOnlyRepository _supplierReadOnlyRepository;

        public GetAllSupplierQueryHandler(ISupplierReadOnlyRepository supplierReadOnlyRepository)
        {
            _supplierReadOnlyRepository = supplierReadOnlyRepository;
        }

        public async Task<PagedResult<GetSupplierResponse>> Handle(GetAllSupplierQuery request, CancellationToken ct)
        {
            var result = await _supplierReadOnlyRepository.GetAllAsync(
                request.CorporateName,
                request.FantasyName,
                request.CNPJ,
                request.Status,
                request.Page,
                request.PageSize,
                ct);

            var items = result.Items
                .Select(s => new GetSupplierResponse
                {
                    Id = s.Id,
                    CorporateName = s.CorporateName,
                    FantasyName = s.FantasyName,
                    CNPJ = s.CNPJ,
                    Phone = s.Phone,
                    Email = s.Email,
                    Status = s.Status
                })
                .ToList();

            return new PagedResult<GetSupplierResponse>(
                items,
                result.Page,
                result.PageSize,
                result.TotalItems,
                result.TotalPages);
        }
    }
}