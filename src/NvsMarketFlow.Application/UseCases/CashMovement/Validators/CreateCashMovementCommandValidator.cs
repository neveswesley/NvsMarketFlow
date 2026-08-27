using FluentValidation;
using NvsMarketFlow.Application.UseCases.CashMovement.Commands;

namespace NvsMarketFlow.Application.UseCases.CashMovement.Validators;

public class CreateCashMovementCommandValidator : AbstractValidator<CreateCashMovement.CreateCashMovementCommand>
{
    public CreateCashMovementCommandValidator()
    {
        RuleFor(x => x.CashRegisterId)
            .NotEmpty().WithMessage("Cash register id is required.");

        RuleFor(x => x.Request.Type)
            .IsInEnum().WithMessage("Invalid movement type.");

        RuleFor(x => x.Request.Value)
            .GreaterThan(0).WithMessage("Value must be greater than zero.");

        RuleFor(x => x.Request.Reason)
            .NotEmpty().WithMessage("Reason is required.")
            .MaximumLength(250).WithMessage("Reason must be at most 250 characters long.");
    }
}