using FluentValidation;
using NvsMarketFlow.Application.UseCases.Product.Commands;

namespace NvsMarketFlow.Application.UseCases.Product.Validators;

public class DeactivateProductCommandValidator : AbstractValidator<DeactivateProduct.DeactivateProductCommand>
{
    public DeactivateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product id is required.");
    }
}