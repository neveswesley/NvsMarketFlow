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

        public UpdateCategoryCommandHandler(ICategoryWriteOnlyRepository categoryWriteOnlyRepository, ICategoryReadOnlyRepository categoryReadOnlyRepository)
        {
            _categoryWriteOnlyRepository = categoryWriteOnlyRepository;
            _categoryReadOnlyRepository = categoryReadOnlyRepository;
        }

        public async Task<Guid> Handle(UpdateCategoryCommand request, CancellationToken ct)
        {
            var category = await _categoryReadOnlyRepository.GetByIdAsync(request.CategoryId, ct);

            if (category == null)
                throw new NotFoundException("Category not found.");
            
            category.UpdateCategory(request.Request.Name);
            
            await _categoryWriteOnlyRepository.UpdateAsync(category);
            return category.Id;
        }
    }
}