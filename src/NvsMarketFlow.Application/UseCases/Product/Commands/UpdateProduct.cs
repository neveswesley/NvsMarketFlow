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
            if (product == null)
                throw new NotFoundException("Product not found");
            
            product.UpdateInfo(command.Request.Sku, 
                command.Request.Barcode, 
                command.Request.Name, 
                command.Request.Description, 
                command.Request.CategoryId, 
                command.Request.BrandId,
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