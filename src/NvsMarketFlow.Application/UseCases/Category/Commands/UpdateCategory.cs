using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Requests.Category;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Category.Commands;

public abstract class UpdateCategory
{
    public sealed record UpdateCategoryCommand(Guid CategoryId, UpdateCategoryRequest Request) : IRequest<Guid>;

    public sealed record UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Guid>
    {
        private readonly ICategoryWriteOnlyRepository _categoryWriteOnlyRepository;
        private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryCommandHandler(ICategoryWriteOnlyRepository categoryWriteOnlyRepository, ICategoryReadOnlyRepository categoryReadOnlyRepository, IUnitOfWork unitOfWork)
        {
            _categoryWriteOnlyRepository = categoryWriteOnlyRepository;
            _categoryReadOnlyRepository = categoryReadOnlyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(UpdateCategoryCommand request, CancellationToken ct)
        {
            var category = await _categoryReadOnlyRepository.GetByIdAsync(request.CategoryId, ct);

            if (category == null)
                throw new NotFoundException("Category not found.");
            
            category.UpdateCategory(request.Request.Name);
            
             _categoryWriteOnlyRepository.UpdateAsync(category);
             
             await _unitOfWork.SaveChangesAsync(ct);
             
            return category.Id;
        }
    }
}