using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Sale.Commands;

public class RemoveSaleItem
{
    public sealed record RemoveSaleItemCommand(Guid SaleId, Guid ItemId) : IRequest;

    public class RemoveSaleItemCommandHandler : IRequestHandler<RemoveSaleItemCommand>
    {
        private readonly ISaleWriteOnlyRepository _saleWriteOnlyRepository;
        private readonly ISaleReadOnlyRepository _saleReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveSaleItemCommandHandler(
            ISaleWriteOnlyRepository saleWriteOnlyRepository,
            ISaleReadOnlyRepository saleReadOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _saleWriteOnlyRepository = saleWriteOnlyRepository;
            _saleReadOnlyRepository = saleReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(RemoveSaleItemCommand command, CancellationToken ct)
        {
            var sale = await _saleReadOnlyRepository.GetByIdAsync(command.SaleId, ct);

            if (sale is null)
                throw new NotFoundException($"Sale with id '{command.SaleId}' not found.");

            sale.RemoveItem(command.ItemId);

            await _saleWriteOnlyRepository.UpdateAsync(sale, ct);

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}