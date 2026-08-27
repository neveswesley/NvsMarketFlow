using FluentValidation;
using NvsMarketFlow.Application.UseCases.CashRegister.Queries;

namespace NvsMarketFlow.Application.UseCases.CashRegister.Validators;

public sealed class GetCashRegisterByIdQueryValidator
    : AbstractValidator<GetCashRegisterById.GetCashRegisterByIdQuery>
{
    public GetCashRegisterByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Cash register id is required.");
    }
}