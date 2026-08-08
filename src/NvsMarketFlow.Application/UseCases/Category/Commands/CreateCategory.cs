using MediatR;
using NvsMarketFlow.Application.Requests.Category;
using NvsMarketFlow.Application.Responses.Category;
using NvsMarketFlow.Domain.Interfaces.WriteOnly;

namespace NvsMarketFlow.Application.UseCases.Category.Commands;

public abstract class CreateCategory
{
    public sealed record CreateCategoryCommand(CreateCategoryRequest Request) : IRequest<CreateCategoryResponse>;

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CreateCategoryResponse>
    {
        private readonly ICategoryWriteOnlyRepository _categoryWriteOnlyRepository;

        public CreateCategoryCommandHandler(ICategoryWriteOnlyRepository categoryWriteOnlyRepository)
        {
            _categoryWriteOnlyRepository = categoryWriteOnlyRepository;
        }

        public async Task<CreateCategoryResponse> Handle(CreateCategoryCommand command,
            CancellationToken cancellationToken)
        {
            var category = new Domain.Entities.Category(command.Request.Name);

            await _categoryWriteOnlyRepository.CreateAsync(category);

            return new CreateCategoryResponse
            {
                Name = category.Name
            };
        }
    }
}