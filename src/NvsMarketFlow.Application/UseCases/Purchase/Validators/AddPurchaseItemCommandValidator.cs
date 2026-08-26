using FluentValidation;
using NvsMarketFlow.Application.UseCases.Purchase.Commands;

namespace NvsMarketFlow.Application.UseCases.Purchase.Validators;

public class AddPurchaseItemCommandValidator : AbstractValidator<AddPurchaseItem.AddPurchaseItemCommand>
{
    public AddPurchaseItemCommandValidator()
    {
        RuleFor(x => x.PurchaseId)
            .NotEmpty().WithMessage("Purchase id is required.");

        RuleFor(x => x.Request.ProductId)
            .NotEmpty().WithMessage("Product id is required.");

        RuleFor(x => x.Request.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

        RuleFor(x => x.Request.CostPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Cost price cannot be negative.");
    }
}