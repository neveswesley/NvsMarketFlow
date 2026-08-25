using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Category.Commands;

public class DeleteCategory
{
    public sealed record DeleteCategoryCommand(Guid CategoryId) : IRequest<Unit>;

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Unit>
    {

        private readonly ICategoryWriteOnlyRepository _categoryWriteOnlyRepository;
        private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryCommandHandler(ICategoryWriteOnlyRepository categoryWriteOnlyRepository, ICategoryReadOnlyRepository categoryReadOnlyRepository, IUnitOfWork unitOfWork)
        {
            _categoryWriteOnlyRepository = categoryWriteOnlyRepository;
            _categoryReadOnlyRepository = categoryReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryReadOnlyRepository.GetByIdAsync(request.CategoryId, cancellationToken);
            
            if (category == null)
                throw new NotFoundException("Category not found.");
            
            var hasProducts = await _categoryReadOnlyRepository.HasLinkedProductsAsync(category.Id, cancellationToken);
            if (hasProducts)
                throw new CategoryHasLinkedProductsException("The category has linked products.");

            _categoryWriteOnlyRepository.DeleteAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}