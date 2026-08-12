using FluentValidation;
using NvsMarketFlow.Application.Requests.Product;

namespace NvsMarketFlow.Application.UseCases.Product.Validators;

public class UpdateProductValidator : AbstractValidator<UpdateProductInfoRequest>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Sku)
            .NotEmpty()
            .WithMessage("SKU is required.")
            .MaximumLength(50)
            .WithMessage("SKU must not exceed 50 characters.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Product name is required.")
            .MaximumLength(150)
            .WithMessage("Product name must not exceed 150 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category is required.");

        RuleFor(x => x.CostPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Cost price cannot be negative.");

        RuleFor(x => x.SalePrice)
            .GreaterThan(0)
            .WithMessage("Sale price must be greater than zero.");

        RuleFor(x => x.SalePrice)
            .GreaterThanOrEqualTo(x => x.CostPrice)
            .WithMessage("Sale price cannot be lower than cost price.");

        RuleFor(x => x.MinimumStock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum stock cannot be negative.");

        RuleFor(x => x.MaximumStock)
            .GreaterThan(0)
            .WithMessage("Maximum stock must be greater than zero.");

        RuleFor(x => x.MaximumStock)
            .GreaterThanOrEqualTo(x => x.MinimumStock)
            .WithMessage("Maximum stock cannot be lower than minimum stock.");

        RuleFor(x => x.Unit)
            .IsInEnum()
            .WithMessage("Invalid unit.");
    }
}