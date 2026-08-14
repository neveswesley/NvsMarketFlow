using FluentValidation;
using NvsMarketFlow.Application.Requests.Brand;

namespace NvsMarketFlow.Application.UseCases.Brand.Validators;

public class CreateBrandValidator : AbstractValidator<CreateBrandRequest>
{
    public CreateBrandValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MinimumLength(2)
            .WithMessage("Name must be at least 2 characters long")
            .MaximumLength(50)
            .WithMessage("Name cannot exceed 50 characters");
    }
}