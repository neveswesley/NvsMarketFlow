using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Requests.Product;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Product.Commands;

public class UpdateProduct
{
    public sealed record UpdateProductCommand (Guid Id, UpdateProductInfoRequest Request) : IRequest<Unit>;

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
    {

        private readonly IProductReadOnlyRepository _productReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IProductReadOnlyRepository productReadOnlyRepository, IUnitOfWork unitOfWork)
        {
            _productReadOnlyRepository = productReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateProductCommand command, CancellationToken ct)
        {
            var product = await _productReadOnlyRepository.GetByIdAsync(command.Id, ct);

            if (product is null)
                throw new NotFoundException($"Product with id '{command.Id}' not found.");

            var existingName = await _productReadOnlyRepository
                .ExistsByNameAsync(command.Request.Name, command.Id, ct);

            if (existingName)
                throw new DuplicateFieldException("Product", "name", command.Request.Name);

            var existingSku = await _productReadOnlyRepository
                .ExistsBySkuAsync(command.Request.Sku, command.Id, ct);

            if (existingSku)
                throw new DuplicateFieldException("Product", "Sku", command.Request.Sku);

            var existingBarcode = await _productReadOnlyRepository
                .ExistsByBarcodeAsync(command.Request.Barcode, command.Id, ct);

            if (existingBarcode)
                throw new DuplicateFieldException("Product", "Barcode", command.Request.Barcode);

            product.UpdateInfo(
                command.Request.Sku,
                command.Request.Barcode,
                command.Request.Name,
                command.Request.Description,
                command.Request.CategoryId,
                command.Request.BrandId,
                command.Request.SupplierId,
                command.Request.CostPrice,
                command.Request.SalePrice,
                command.Request.MinimumStock,
                command.Request.MaximumStock,
                command.Request.Unit);
            
            await _unitOfWork.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
    
}