using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Sale.Commands;

public class CancelSale
{
    public sealed record CancelSaleCommand(Guid Id) : IRequest;

    public class CancelSaleCommandHandler : IRequestHandler<CancelSaleCommand>
    {
        private readonly ISaleReadOnlyRepository _saleReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CancelSaleCommandHandler(
            ISaleReadOnlyRepository saleReadOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _saleReadOnlyRepository = saleReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CancelSaleCommand command, CancellationToken ct)
        {
            var sale = await _saleReadOnlyRepository.GetByIdAsync(command.Id, ct);

            if (sale is null)
                throw new NotFoundException($"Sale with id '{command.Id}' not found.");

            sale.Cancel();

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}