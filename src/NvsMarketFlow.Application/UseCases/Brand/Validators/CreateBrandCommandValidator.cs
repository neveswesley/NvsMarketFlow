using FluentValidation;
using NvsMarketFlow.Application.UseCases.Brand.Commands;

namespace NvsMarketFlow.Application.UseCases.Brand.Validators;

public class CreateBrandCommandValidator : AbstractValidator<CreateBrand.CreateBrandCommand>
{
    public CreateBrandCommandValidator()
    {
        RuleFor(x => x.Request)
            .NotNull()
            .WithMessage("Request body is required.")
            .SetValidator(new CreateBrandValidator());
    }
}