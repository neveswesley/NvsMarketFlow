using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Requests.CashMovement;
using NvsMarketFlow.Application.Responses.CashMovement;
using NvsMarketFlow.Domain.Enums;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.CashMovement.Commands;

public class CreateCashMovement
{
    public sealed record CreateCashMovementCommand(Guid CashRegisterId, CreateCashMovementRequest Request)
        : IRequest<CreateCashMovementResponse>;

    public class CreateCashMovementCommandHandler : IRequestHandler<CreateCashMovementCommand, CreateCashMovementResponse>
    {
        private readonly ICashMovementWriteOnlyRepository _cashMovementWriteOnlyRepository;
        private readonly ICashRegisterReadOnlyRepository _cashRegisterReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCashMovementCommandHandler(
            ICashMovementWriteOnlyRepository cashMovementWriteOnlyRepository,
            ICashRegisterReadOnlyRepository cashRegisterReadOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _cashMovementWriteOnlyRepository = cashMovementWriteOnlyRepository;
            _cashRegisterReadOnlyRepository = cashRegisterReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateCashMovementResponse> Handle(CreateCashMovementCommand command, CancellationToken ct)
        {
            var cashRegister = await _cashRegisterReadOnlyRepository.GetByIdAsync(command.CashRegisterId, ct);

            if (cashRegister is null)
                throw new NotFoundException($"Cash register with id '{command.CashRegisterId}' not found.");

            if (cashRegister.Status != CashRegisterStatus.Open)
                throw new InvalidOperationException("Cannot add movements to a closed cash register.");

            var cashMovement = new Domain.Entities.CashMovement(
                command.CashRegisterId,
                command.Request.Type,
                command.Request.Value,
                command.Request.Reason);

            await _cashMovementWriteOnlyRepository.CreateAsync(cashMovement, ct);

            await _unitOfWork.SaveChangesAsync(ct);

            return new CreateCashMovementResponse
            {
                Id = cashMovement.Id,
                CashRegisterId = cashMovement.CashRegisterId,
                Type = cashMovement.Type,
                Value = cashMovement.Value,
                Reason = cashMovement.Reason,
                CreatedAt = cashMovement.CreatedAt
            };
        }
    }
}