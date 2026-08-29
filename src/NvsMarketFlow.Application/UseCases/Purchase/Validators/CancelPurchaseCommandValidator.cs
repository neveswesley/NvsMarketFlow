using FluentValidation;
using NvsMarketFlow.Application.UseCases.Purchase.Commands;

namespace NvsMarketFlow.Application.UseCases.Purchase.Validators;

public class CancelPurchaseCommandValidator : AbstractValidator<CancelPurchase.CancelPurchaseCommand>
{
    public CancelPurchaseCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Purchase id is required.");
    }
}