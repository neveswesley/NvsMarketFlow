using MediatR;
using NvsMarketFlow.Application.Events;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Requests.CashRegister;
using NvsMarketFlow.Application.Responses.CashRegister;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.CashRegister.Commands;

public class CloseCashRegister
{
    public sealed record CloseCashRegisterCommand(Guid Id, CloseCashRegisterRequest Request)
        : IRequest<CloseCashRegisterResponse>;

    public class CloseCashRegisterCommandHandler : IRequestHandler<CloseCashRegisterCommand, CloseCashRegisterResponse>
    {
        private readonly ICashRegisterReadOnlyRepository _cashRegisterReadOnlyRepository;
        private readonly ICashMovementReadOnlyRepository _cashMovementReadOnlyRepository;
        private readonly IPublisher _publisher;
        private readonly IUnitOfWork _unitOfWork;

        public CloseCashRegisterCommandHandler(ICashRegisterReadOnlyRepository cashRegisterReadOnlyRepository, ICashMovementReadOnlyRepository cashMovementReadOnlyRepository, IPublisher publisher, IUnitOfWork unitOfWork)
        {
            _cashRegisterReadOnlyRepository = cashRegisterReadOnlyRepository;
            _cashMovementReadOnlyRepository = cashMovementReadOnlyRepository;
            _publisher = publisher;
            _unitOfWork = unitOfWork;
        }

        public async Task<CloseCashRegisterResponse> Handle(CloseCashRegisterCommand command, CancellationToken ct)
        {
            var cashRegister = await _cashRegisterReadOnlyRepository.GetByIdAsync(command.Id, ct);

            if (cashRegister is null)
                throw new NotFoundException($"Cash register with id '{command.Id}' not found.");

            var movements = await _cashMovementReadOnlyRepository
                .GetAllByCashRegisterIdAsync(command.Id, ct);

            var expectedBalance = cashRegister.OpeningBalance +
                movements.Sum(m => m.IsIncrease ? m.Value : -m.Value);

            var discrepancy = cashRegister.Close(command.Request.ClosingBalance, expectedBalance);
            
            if (discrepancy != 0)
            {
                await _publisher.Publish(new CashRegisterDiscrepancyEvent(
                    cashRegister.Id,
                    cashRegister.UserId,
                    discrepancy), ct);
            }

            await _unitOfWork.SaveChangesAsync(ct);

            return new CloseCashRegisterResponse
            {
                Id = cashRegister.Id,
                UserId = cashRegister.UserId,
                OpeningBalance = cashRegister.OpeningBalance,
                ExpectedBalance = expectedBalance,
                ClosingBalance = cashRegister.ClosingBalance!.Value,
                Discrepancy = discrepancy,
                Status = cashRegister.Status,
                OpenedAt = cashRegister.OpenedAt,
                ClosedAt = cashRegister.ClosedAt!.Value
            };
        }
    }
}