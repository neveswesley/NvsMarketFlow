using FluentValidation;
using NvsMarketFlow.Application.UseCases.CashMovement.Queries;

namespace NvsMarketFlow.Application.UseCases.CashMovement.Validators;

public sealed class GetAllCashMovementQueryValidator
    : AbstractValidator<GetAllCashMovement.GetAllCashMovementQuery>
{
    public GetAllCashMovementQueryValidator()
    {
        RuleFor(x => x.CashRegisterId)
            .NotEmpty().WithMessage("Cash register id is required.");

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid movement type.")
            .When(x => x.Type.HasValue);
    }
}