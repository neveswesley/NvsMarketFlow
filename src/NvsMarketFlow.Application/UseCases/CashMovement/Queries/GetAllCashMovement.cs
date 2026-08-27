using MediatR;
using NvsMarketFlow.Application.Common;
using NvsMarketFlow.Application.Responses.CashMovement;
using NvsMarketFlow.Domain.Enums;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;

namespace NvsMarketFlow.Application.UseCases.CashMovement.Queries;

public class GetAllCashMovement
{
    public sealed record GetAllCashMovementQuery(
        Guid CashRegisterId,
        CashMovementType? Type,
        int Page = 1,
        int PageSize = 10
    ) : IRequest<PagedResult<GetCashMovementResponse>>;

    public class GetAllCashMovementQueryHandler : IRequestHandler<GetAllCashMovementQuery, PagedResult<GetCashMovementResponse>>
    {
        private readonly ICashMovementReadOnlyRepository _cashMovementReadOnlyRepository;

        public GetAllCashMovementQueryHandler(ICashMovementReadOnlyRepository cashMovementReadOnlyRepository)
        {
            _cashMovementReadOnlyRepository = cashMovementReadOnlyRepository;
        }

        public async Task<PagedResult<GetCashMovementResponse>> Handle(GetAllCashMovementQuery request, CancellationToken ct)
        {
            var result = await _cashMovementReadOnlyRepository.GetAllAsync(
                request.CashRegisterId,
                request.Type,
                request.Page,
                request.PageSize,
                ct);

            var items = result.Items
                .Select(cm => new GetCashMovementResponse
                {
                    Id = cm.Id,
                    Type = cm.Type,
                    Value = cm.Value,
                    Reason = cm.Reason,
                    CreatedAt = cm.CreatedAt
                })
                .ToList();

            return new PagedResult<GetCashMovementResponse>(
                items,
                result.Page,
                result.PageSize,
                result.TotalItems,
                result.TotalPages);
        }
    }
}