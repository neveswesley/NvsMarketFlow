using FluentValidation;
using NvsMarketFlow.Application.UseCases.Brand.Commands;

namespace NvsMarketFlow.Application.UseCases.Brand.Validators;

public class UpdateBrandCommandValidator : AbstractValidator<UpdateBrand.UpdateBrandCommand>
{
    public UpdateBrandCommandValidator()
    {
        RuleFor(x => x.Request)
            .NotNull()
            .WithMessage("Request body is required.")
            .SetValidator(new UpdateBrandValidator());
    }
}