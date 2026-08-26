using FluentValidation;
using NvsMarketFlow.Application.UseCases.Purchase.Commands;

namespace NvsMarketFlow.Application.UseCases.Purchase.Validators;

public class RemovePurchaseItemCommandValidator : AbstractValidator<RemovePurchaseItem.RemovePurchaseItemCommand>
{
    public RemovePurchaseItemCommandValidator()
    {
        RuleFor(x => x.PurchaseId)
            .NotEmpty().WithMessage("Purchase id is required.");

        RuleFor(x => x.ItemId)
            .NotEmpty().WithMessage("Item id is required.");
    }
}