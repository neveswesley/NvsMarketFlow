using FluentValidation;
using static NvsMarketFlow.Application.UseCases.Product.Commands.UpdateProduct;

namespace NvsMarketFlow.Application.UseCases.Product.Validators;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product id is required.");

        RuleFor(x => x.Request)
            .NotNull()
            .WithMessage("Request body is required.")
            .SetValidator(new UpdateProductValidator());
    }
}