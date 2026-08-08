using FluentValidation;
using NvsMarketFlow.Application.Requests.Category;
using NvsMarketFlow.Application.UseCases.Category.Commands;

namespace NvsMarketFlow.Application.UseCases.Category.Validators;

public class CreateCategoryValidator : AbstractValidator<CreateCategory.CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(c => c.Request.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Category name is required.")
            .MinimumLength(2)
            .WithMessage("Category name must contain at least 2 characters.");
    }
}