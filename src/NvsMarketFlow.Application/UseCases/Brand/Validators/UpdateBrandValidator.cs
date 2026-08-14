using FluentValidation;
using NvsMarketFlow.Application.Requests.Brand;

namespace NvsMarketFlow.Application.UseCases.Brand.Validators;

public class UpdateBrandValidator : AbstractValidator<UpdateBrandRequest>
{
    public UpdateBrandValidator()
    {
        RuleFor(b => b.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MinimumLength(2)
            .WithMessage("Name must be at least 2 characters long")
            .MaximumLength(50)
            .WithMessage("Name must be less than 50 characters long");
    }
}