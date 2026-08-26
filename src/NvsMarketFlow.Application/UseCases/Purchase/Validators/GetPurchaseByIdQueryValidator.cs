using FluentValidation;
using NvsMarketFlow.Application.UseCases.Purchase.Queries;

namespace NvsMarketFlow.Application.UseCases.Purchase.Validators;

public sealed class GetPurchaseByIdQueryValidator
    : AbstractValidator<GetPurchaseById.GetPurchaseByIdQuery>
{
    public GetPurchaseByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Purchase id is required.");
    }
}