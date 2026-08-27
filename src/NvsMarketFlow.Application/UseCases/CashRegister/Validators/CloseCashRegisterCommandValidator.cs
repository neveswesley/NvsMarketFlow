using FluentValidation;
using NvsMarketFlow.Application.UseCases.CashRegister.Commands;

namespace NvsMarketFlow.Application.UseCases.CashRegister.Validators;

public class CloseCashRegisterCommandValidator : AbstractValidator<CloseCashRegister.CloseCashRegisterCommand>
{
    public CloseCashRegisterCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Cash register id is required.");

        RuleFor(x => x.Request.ClosingBalance)
            .GreaterThanOrEqualTo(0).WithMessage("Closing balance cannot be negative.");
    }
}