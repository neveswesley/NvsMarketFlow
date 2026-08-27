using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Requests.CashRegister;
using NvsMarketFlow.Application.Responses.CashRegister;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.CashRegister.Commands;

public class OpenCashRegister
{
    public sealed record OpenCashRegisterCommand(OpenCashRegisterRequest Request) : IRequest<OpenCashRegisterResponse>;

    public class OpenCashRegisterCommandHandler : IRequestHandler<OpenCashRegisterCommand, OpenCashRegisterResponse>
    {
        private readonly ICashRegisterWriteOnlyRepository _cashRegisterWriteOnlyRepository;
        private readonly ICashRegisterReadOnlyRepository _cashRegisterReadOnlyRepository;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OpenCashRegisterCommandHandler(
            ICashRegisterWriteOnlyRepository cashRegisterWriteOnlyRepository,
            ICashRegisterReadOnlyRepository cashRegisterReadOnlyRepository,
            IUserReadOnlyRepository userReadOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _cashRegisterWriteOnlyRepository = cashRegisterWriteOnlyRepository;
            _cashRegisterReadOnlyRepository = cashRegisterReadOnlyRepository;
            _userReadOnlyRepository = userReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<OpenCashRegisterResponse> Handle(OpenCashRegisterCommand command, CancellationToken ct)
        {
            var user = await _userReadOnlyRepository.GetByIdAsync(command.Request.UserId, ct);

            if (user is null)
                throw new NotFoundException($"User with id '{command.Request.UserId}' not found.");

            var hasOpenCashRegister = await _cashRegisterReadOnlyRepository
                .HasOpenCashRegisterAsync(command.Request.UserId, ct);

            if (hasOpenCashRegister)
                throw new InvalidOperationException("User already has an open cash register.");

            var cashRegister = new Domain.Entities.CashRegister(
                command.Request.UserId,
                command.Request.OpeningBalance);

            await _cashRegisterWriteOnlyRepository.CreateAsync(cashRegister, ct);

            await _unitOfWork.SaveChangesAsync(ct);

            return new OpenCashRegisterResponse
            {
                Id = cashRegister.Id,
                UserId = cashRegister.UserId,
                OpeningBalance = cashRegister.OpeningBalance,
                Status = cashRegister.Status,
                OpenedAt = cashRegister.OpenedAt
            };
        }
    }
}