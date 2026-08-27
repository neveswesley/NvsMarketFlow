using FluentValidation;
using NvsMarketFlow.Application.UseCases.CashRegister.Queries;

namespace NvsMarketFlow.Application.UseCases.CashRegister.Validators;

public sealed class GetAllCashRegisterQueryValidator
    : AbstractValidator<GetAllCashRegister.GetAllCashRegisterQuery>
{
    public GetAllCashRegisterQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User id must be valid.")
            .When(x => x.UserId.HasValue);

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid status.")
            .When(x => x.Status.HasValue);

        RuleFor(x => x)
            .Must(x => !x.StartDate.HasValue ||
                       !x.EndDate.HasValue ||
                       x.StartDate <= x.EndDate)
            .WithMessage("Start date cannot be greater than end date.");
    }
}