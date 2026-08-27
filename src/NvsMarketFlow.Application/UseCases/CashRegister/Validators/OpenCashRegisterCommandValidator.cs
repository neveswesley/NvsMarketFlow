using FluentValidation;
using NvsMarketFlow.Application.UseCases.CashRegister.Commands;

namespace NvsMarketFlow.Application.UseCases.CashRegister.Validators;

public class OpenCashRegisterCommandValidator : AbstractValidator<OpenCashRegister.OpenCashRegisterCommand>
{
    public OpenCashRegisterCommandValidator()
    {
        RuleFor(x => x.Request.UserId)
            .NotEmpty().WithMessage("User id is required.");

        RuleFor(x => x.Request.OpeningBalance)
            .GreaterThanOrEqualTo(0).WithMessage("Opening balance cannot be negative.");
    }
}