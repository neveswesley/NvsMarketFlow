using FluentValidation;
using NvsMarketFlow.Application.UseCases.Product.Commands;

namespace NvsMarketFlow.Application.UseCases.Product.Validators;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProduct.DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product id is required.");
    }
}