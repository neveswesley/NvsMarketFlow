using FluentValidation;
using NvsMarketFlow.Application.UseCases.Product.Commands;

namespace NvsMarketFlow.Application.UseCases.Product.Validators;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProduct.UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Product id is required.");

        RuleFor(x => x.Request.Sku)
            .NotEmpty().WithMessage("SKU is required.")
            .MaximumLength(50).WithMessage("SKU must be at most 50 characters long.");

        RuleFor(x => x.Request.Barcode)
            .NotEmpty().WithMessage("Barcode is required.")
            .MaximumLength(50).WithMessage("Barcode must be at most 50 characters long.");

        RuleFor(x => x.Request.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MinimumLength(3).WithMessage("Name must be at least 3 characters long.")
            .MaximumLength(150).WithMessage("Name must be at most 150 characters long.");

        RuleFor(x => x.Request.Description)
            .MaximumLength(500).WithMessage("Description must be at most 500 characters long.");

        RuleFor(x => x.Request.CategoryId)
            .NotEmpty().WithMessage("Category is required.");

        RuleFor(x => x.Request.BrandId)
            .NotEmpty().WithMessage("Brand id must be valid.")
            .When(x => x.Request.BrandId.HasValue);

        RuleFor(x => x.Request.CostPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Cost price must be zero or greater.");

        RuleFor(x => x.Request.SalePrice)
            .GreaterThan(0).WithMessage("Sale price must be greater than zero.");

        RuleFor(x => x.Request.MinimumStock)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum stock must be zero or greater.");

        RuleFor(x => x.Request.MaximumStock)
            .GreaterThanOrEqualTo(0).WithMessage("Maximum stock must be zero or greater.")
            .GreaterThanOrEqualTo(x => x.Request.MinimumStock)
            .WithMessage("Maximum stock must be greater than or equal to minimum stock.");

        RuleFor(x => x.Request.Unit)
            .IsInEnum().WithMessage("Invalid unit.");
    }
}