using FluentValidation;
using NvsMarketFlow.Application.UseCases.Sale.Commands;

namespace NvsMarketFlow.Application.UseCases.Sale.Validators;

public class AddSaleItemCommandValidator : AbstractValidator<AddSaleItem.AddSaleItemCommand>
{
    public AddSaleItemCommandValidator()
    {
        RuleFor(x => x.SaleId)
            .NotEmpty().WithMessage("Sale id is required.");

        RuleFor(x => x.Request.ProductId)
            .NotEmpty().WithMessage("Product id is required.");

        RuleFor(x => x.Request.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

        RuleFor(x => x.Request.Discount)
            .GreaterThanOrEqualTo(0).WithMessage("Discount cannot be negative.");
    }
}