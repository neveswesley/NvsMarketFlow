using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Requests.Sale;
using NvsMarketFlow.Application.Responses.Sale;
using NvsMarketFlow.Domain.Enums;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Sale.Commands;

public class CreateSale
{
    public sealed record CreateSaleCommand(CreateSaleRequest Request) : IRequest<CreateSaleResponse>;

    public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, CreateSaleResponse>
    {
        private readonly ISaleWriteOnlyRepository _saleWriteOnlyRepository;
        private readonly ISaleReadOnlyRepository _saleReadOnlyRepository;
        private readonly ICashRegisterReadOnlyRepository _cashRegisterReadOnlyRepository;
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSaleCommandHandler(
            ISaleWriteOnlyRepository saleWriteOnlyRepository,
            ISaleReadOnlyRepository saleReadOnlyRepository,
            ICashRegisterReadOnlyRepository cashRegisterReadOnlyRepository,
            IUserReadOnlyRepository userReadOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _saleWriteOnlyRepository = saleWriteOnlyRepository;
            _saleReadOnlyRepository = saleReadOnlyRepository;
            _cashRegisterReadOnlyRepository = cashRegisterReadOnlyRepository;
            _userReadOnlyRepository = userReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateSaleResponse> Handle(CreateSaleCommand command, CancellationToken ct)
        {
            var cashRegister = await _cashRegisterReadOnlyRepository
                .GetByIdAsync(command.Request.CashRegisterId, ct);

            if (cashRegister is null)
                throw new NotFoundException($"Cash register with id '{command.Request.CashRegisterId}' not found.");

            if (cashRegister.Status != CashRegisterStatus.Open)
                throw new InvalidOperationException("Cannot start a sale on a closed cash register.");

            var seller = await _userReadOnlyRepository.GetByIdAsync(command.Request.SellerId, ct);

            if (seller is null)
                throw new NotFoundException($"User with id '{command.Request.SellerId}' not found.");

            var nextNumber = await _saleReadOnlyRepository.GetNextSaleNumberAsync(ct);
            var saleNumber = nextNumber.ToString("D6");

            var sale = new Domain.Entities.Sale(
                command.Request.CashRegisterId,
                command.Request.SellerId,
                saleNumber);

            await _saleWriteOnlyRepository.CreateAsync(sale, ct);

            await _unitOfWork.SaveChangesAsync(ct);

            return new CreateSaleResponse
            {
                Id = sale.Id,
                CashRegisterId = sale.CashRegisterId,
                SellerId = sale.SellerId,
                SaleNumber = sale.SaleNumber,
                Total = sale.Total,
                Status = sale.Status
            };
        }
    }
}