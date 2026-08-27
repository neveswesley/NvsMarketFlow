using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Responses.Sale;
using NvsMarketFlow.Domain.Enums;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Sale.Commands;

public class FinalizeSale
{
    public sealed record FinalizeSaleCommand(Guid Id) : IRequest<FinalizeSaleResponse>;

    public class FinalizeSaleCommandHandler : IRequestHandler<FinalizeSaleCommand, FinalizeSaleResponse>
    {
        private readonly ISaleReadOnlyRepository _saleReadOnlyRepository;
        private readonly IStockMovementWriteOnlyRepository _stockMovementWriteOnlyRepository;
        private readonly ICashMovementWriteOnlyRepository _cashMovementWriteOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FinalizeSaleCommandHandler(
            ISaleReadOnlyRepository saleReadOnlyRepository,
            IStockMovementWriteOnlyRepository stockMovementWriteOnlyRepository,
            ICashMovementWriteOnlyRepository cashMovementWriteOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _saleReadOnlyRepository = saleReadOnlyRepository;
            _stockMovementWriteOnlyRepository = stockMovementWriteOnlyRepository;
            _cashMovementWriteOnlyRepository = cashMovementWriteOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<FinalizeSaleResponse> Handle(FinalizeSaleCommand command, CancellationToken ct)
        {
            var sale = await _saleReadOnlyRepository.GetByIdAsync(command.Id, ct);

            if (sale is null)
                throw new NotFoundException($"Sale with id '{command.Id}' not found.");

            sale.Finalize();

            foreach (var item in sale.Items)
            {
                item.Product.ApplyStockMovement(MovementType.Sale, item.Quantity);

                var stockMovement = new Domain.Entities.StockMovement(
                    item.ProductId,
                    sale.SellerId,
                    MovementType.Sale,
                    item.Quantity,
                    $"Sale finalized - {sale.SaleNumber}");

                await _stockMovementWriteOnlyRepository.CreateAsync(stockMovement, ct);
            }

            var cashMovement = new Domain.Entities.CashMovement(
                sale.CashRegisterId,
                CashMovementType.Sale,
                sale.Total,
                $"Sale finalized - {sale.SaleNumber}");

            await _cashMovementWriteOnlyRepository.CreateAsync(cashMovement, ct);

            await _unitOfWork.SaveChangesAsync(ct);

            return new FinalizeSaleResponse
            {
                Id = sale.Id,
                SaleNumber = sale.SaleNumber,
                Total = sale.Total,
                Status = sale.Status
            };
        }
    }
}