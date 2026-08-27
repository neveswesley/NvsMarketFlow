using MediatR;
using NvsMarketFlow.Application.Common;
using NvsMarketFlow.Application.Responses.CashRegister;
using NvsMarketFlow.Domain.Enums;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;

namespace NvsMarketFlow.Application.UseCases.CashRegister.Queries;

public class GetAllCashRegister
{
    public sealed record GetAllCashRegisterQuery(
        Guid? UserId,
        CashRegisterStatus? Status,
        DateTime? StartDate,
        DateTime? EndDate,
        int Page = 1,
        int PageSize = 10
    ) : IRequest<PagedResult<GetCashRegisterResponse>>;

    public class GetAllCashRegisterQueryHandler : IRequestHandler<GetAllCashRegisterQuery, PagedResult<GetCashRegisterResponse>>
    {
        private readonly ICashRegisterReadOnlyRepository _cashRegisterReadOnlyRepository;

        public GetAllCashRegisterQueryHandler(ICashRegisterReadOnlyRepository cashRegisterReadOnlyRepository)
        {
            _cashRegisterReadOnlyRepository = cashRegisterReadOnlyRepository;
        }

        public async Task<PagedResult<GetCashRegisterResponse>> Handle(GetAllCashRegisterQuery request, CancellationToken ct)
        {
            var result = await _cashRegisterReadOnlyRepository.GetAllAsync(
                request.UserId,
                request.Status,
                request.StartDate,
                request.EndDate,
                request.Page,
                request.PageSize,
                ct);

            var items = result.Items
                .Select(c => new GetCashRegisterResponse
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    UserName = c.User.Name,
                    OpeningBalance = c.OpeningBalance,
                    ClosingBalance = c.ClosingBalance,
                    Status = c.Status,
                    OpenedAt = c.OpenedAt,
                    ClosedAt = c.ClosedAt
                })
                .ToList();

            return new PagedResult<GetCashRegisterResponse>(
                items,
                result.Page,
                result.PageSize,
                result.TotalItems,
                result.TotalPages);
        }
    }
}