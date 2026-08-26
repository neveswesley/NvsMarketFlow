using FluentValidation;
using NvsMarketFlow.Application.UseCases.Purchase.Commands;

namespace NvsMarketFlow.Application.UseCases.Purchase.Validators;

public class ConfirmPurchaseCommandValidator : AbstractValidator<ConfirmPurchase.ConfirmPurchaseCommand>
{
    public ConfirmPurchaseCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Purchase id is required.");

        RuleFor(x => x.Request.UserId)
            .NotEmpty().WithMessage("User id is required.");
    }
}