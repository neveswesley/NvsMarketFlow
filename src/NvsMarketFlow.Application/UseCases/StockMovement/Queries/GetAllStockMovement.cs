using MediatR;
using NvsMarketFlow.Application.Common;
using NvsMarketFlow.Application.Responses.StockMovement;
using NvsMarketFlow.Domain.Enums;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;

namespace NvsMarketFlow.Application.UseCases.StockMovement.Queries;

public class GetAllStockMovement
{
    public sealed record GetAllStockMovementQuery(
        Guid? ProductId,
        Guid? UserId,
        MovementType? MovementType,
        DateTime? StartDate,
        DateTime? EndDate,
        int Page = 1,
        int PageSize = 10
    ) : IRequest<PagedResult<GetStockMovementResponse>>;

    public class GetAllStockMovementQueryHandler : IRequestHandler<GetAllStockMovementQuery, PagedResult<GetStockMovementResponse>>
    {
        private readonly IStockMovementReadOnlyRepository _stockMovementReadOnlyRepository;

        public GetAllStockMovementQueryHandler(IStockMovementReadOnlyRepository stockMovementReadOnlyRepository)
        {
            _stockMovementReadOnlyRepository = stockMovementReadOnlyRepository;
        }

        public async Task<PagedResult<GetStockMovementResponse>> Handle(GetAllStockMovementQuery request, CancellationToken ct)
        {
            var result = await _stockMovementReadOnlyRepository.GetAllAsync(
                request.ProductId,
                request.UserId,
                request.MovementType,
                request.StartDate,
                request.EndDate,
                request.Page,
                request.PageSize,
                ct);

            var items = result.Items
                .Select(sm => new GetStockMovementResponse
                {
                    Id = sm.Id,
                    ProductId = sm.ProductId,
                    ProductName = sm.Product.Name,
                    MovementType = sm.MovementType,
                    Quantity = sm.Quantity,
                    Reason = sm.Reason,
                    UserId = sm.UserId,
                    UserName = sm.User.Name,
                    Date = sm.Date
                })
                .ToList();

            return new PagedResult<GetStockMovementResponse>(
                items,
                result.Page,
                result.PageSize,
                result.TotalItems,
                result.TotalPages);
        }
    }
}