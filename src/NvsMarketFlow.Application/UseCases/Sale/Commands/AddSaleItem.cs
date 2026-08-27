using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Requests.Sale;
using NvsMarketFlow.Application.Responses.Sale;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Sale.Commands;

public class AddSaleItem
{
    public sealed record AddSaleItemCommand(Guid SaleId, AddSaleItemRequest Request)
        : IRequest<AddSaleItemResponse>;

    public class AddSaleItemCommandHandler : IRequestHandler<AddSaleItemCommand, AddSaleItemResponse>
    {
        private readonly ISaleWriteOnlyRepository _saleWriteOnlyRepository;
        private readonly ISaleReadOnlyRepository _saleReadOnlyRepository;
        private readonly IProductReadOnlyRepository _productReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddSaleItemCommandHandler(
            ISaleWriteOnlyRepository saleWriteOnlyRepository,
            ISaleReadOnlyRepository saleReadOnlyRepository,
            IProductReadOnlyRepository productReadOnlyRepository,
            IUnitOfWork unitOfWork)
        {
            _saleWriteOnlyRepository = saleWriteOnlyRepository;
            _saleReadOnlyRepository = saleReadOnlyRepository;
            _productReadOnlyRepository = productReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AddSaleItemResponse> Handle(AddSaleItemCommand command, CancellationToken ct)
        {
            var sale = await _saleReadOnlyRepository.GetByIdAsync(command.SaleId, ct);

            if (sale is null)
                throw new NotFoundException($"Sale with id '{command.SaleId}' not found.");

            var product = await _productReadOnlyRepository.GetByIdAsync(command.Request.ProductId, ct);

            if (product is null)
                throw new NotFoundException($"Product with id '{command.Request.ProductId}' not found.");

            if (product.CurrentStock < command.Request.Quantity)
                throw new InvalidOperationException(
                    $"Insufficient stock for product '{product.Name}'. Available: {product.CurrentStock}, requested: {command.Request.Quantity}.");

            var item = sale.AddItem(
                command.Request.ProductId,
                command.Request.Quantity,
                product.SalePrice,
                command.Request.Discount);

            await _saleWriteOnlyRepository.AddItemAsync(item, ct);

            await _unitOfWork.SaveChangesAsync(ct);

            return new AddSaleItemResponse
            {
                SaleId = sale.Id,
                ProductId = command.Request.ProductId,
                Quantity = command.Request.Quantity,
                UnitPrice = product.SalePrice,
                Discount = command.Request.Discount,
                ItemTotal = item.Total,
                SaleSubtotal = sale.Subtotal,
                SaleTotal = sale.Total
            };
        }
    }
}