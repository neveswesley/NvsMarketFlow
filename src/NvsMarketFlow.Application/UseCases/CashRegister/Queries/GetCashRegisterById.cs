using MediatR;
using NvsMarketFlow.Application.Common;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Responses.CashRegister;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;

namespace NvsMarketFlow.Application.UseCases.CashRegister.Queries;

public class GetCashRegisterById
{
    public sealed record GetCashRegisterByIdQuery(Guid Id) : IRequest<GetCashRegisterByIdResponse>;

    public class GetCashRegisterByIdQueryHandler : IRequestHandler<GetCashRegisterByIdQuery, GetCashRegisterByIdResponse>
    {
        private readonly ICashRegisterReadOnlyRepository _cashRegisterReadOnlyRepository;
        private readonly ICurrentUserContext _currentUserContext;

        public GetCashRegisterByIdQueryHandler(
            ICashRegisterReadOnlyRepository cashRegisterReadOnlyRepository,
            ICurrentUserContext currentUserContext)
        {
            _cashRegisterReadOnlyRepository = cashRegisterReadOnlyRepository;
            _currentUserContext = currentUserContext;
        }

        public async Task<GetCashRegisterByIdResponse> Handle(GetCashRegisterByIdQuery request, CancellationToken ct)
        {
            var cashRegister = await _cashRegisterReadOnlyRepository.GetByIdAsync(request.Id, ct);

            if (cashRegister is null)
                throw new NotFoundException($"Cash register with id '{request.Id}' not found.");

            var isSupervisor = _currentUserContext.Role == "Supervisor";
            if (!isSupervisor && !_currentUserContext.IsOwnerOrAdmin(cashRegister.UserId))
                throw new ForbiddenException("You do not have access to this cash register.");

            return new GetCashRegisterByIdResponse
            {
                Id = cashRegister.Id,
                UserId = cashRegister.UserId,
                UserName = cashRegister.User.Name,
                OpeningBalance = cashRegister.OpeningBalance,
                ClosingBalance = cashRegister.ClosingBalance,
                Status = cashRegister.Status,
                OpenedAt = cashRegister.OpenedAt,
                ClosedAt = cashRegister.ClosedAt
            };
        }
    }
}