using MediatR;
using NvsMarketFlow.Application.Exceptions;
using NvsMarketFlow.Application.Requests.Category;
using NvsMarketFlow.Application.Responses.Category;
using NvsMarketFlow.Domain.Interfaces.ReadOnly;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Category.Commands;

public abstract class CreateCategory
{
    public sealed record CreateCategoryCommand(CreateCategoryRequest Request) : IRequest<CreateCategoryResponse>;

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CreateCategoryResponse>
    {
        private readonly ICategoryWriteOnlyRepository _categoryWriteOnlyRepository;
        private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;

        public CreateCategoryCommandHandler(ICategoryWriteOnlyRepository categoryWriteOnlyRepository, 
            ICategoryReadOnlyRepository categoryReadOnlyRepository)
        {
            _categoryWriteOnlyRepository = categoryWriteOnlyRepository;
            _categoryReadOnlyRepository = categoryReadOnlyRepository;
        }

        public async Task<CreateCategoryResponse> Handle(CreateCategoryCommand command,
            CancellationToken cancellationToken)
        {
            
            var nameExists = await _categoryReadOnlyRepository.ExistsByNameAsync(command.Request.Name, cancellationToken);

            if (nameExists)
                throw new DuplicateFieldException("Category", "name", command.Request.Name);
            
            var category = new Domain.Entities.Category(command.Request.Name);

            await _categoryWriteOnlyRepository.CreateAsync(category);

            return new CreateCategoryResponse
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}