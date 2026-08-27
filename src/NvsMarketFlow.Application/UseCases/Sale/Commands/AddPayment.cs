using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Requests.Sale;
using NvsMarketFlow.Application.Responses.Sale;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Sale.Commands;

public class AddPayment
{
    public sealed record AddPaymentCommand(Guid SaleId, AddPaymentRequest Request) : IRequest<AddPaymentResponse>;

    public class AddPaymentCommandHandler : IRequestHandler<AddPaymentCommand, AddPaymentResponse>
    {
        private readonly ISaleWriteOnlyRepository _saleWriteOnlyRepository;
        private readonly ISaleReadOnlyRepository _saleReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddPaymentCommandHandler(
            ISaleWriteOnlyRepository saleWriteOnlyRepository,
            ISaleReadOnlyRepository saleReadOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _saleWriteOnlyRepository = saleWriteOnlyRepository;
            _saleReadOnlyRepository = saleReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AddPaymentResponse> Handle(AddPaymentCommand command, CancellationToken ct)
        {
            var sale = await _saleReadOnlyRepository.GetByIdAsync(command.SaleId, ct);

            if (sale is null)
                throw new NotFoundException($"Sale with id '{command.SaleId}' not found.");

            var payment = sale.AddPayment(command.Request.Method, command.Request.Value);

            await _saleWriteOnlyRepository.AddPaymentAsync(payment, ct);

            await _unitOfWork.SaveChangesAsync(ct);

            return new AddPaymentResponse
            {
                SaleId = sale.Id,
                Method = payment.Method,
                Value = payment.Value,
                TotalPaid = sale.TotalPaid,
                RemainingAmount = sale.RemainingAmount
            };
        }
    }
}