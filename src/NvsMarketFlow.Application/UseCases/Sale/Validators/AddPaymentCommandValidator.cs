using FluentValidation;
using NvsMarketFlow.Application.UseCases.Sale.Commands;

namespace NvsMarketFlow.Application.UseCases.Sale.Validators;

public class AddPaymentCommandValidator : AbstractValidator<AddPayment.AddPaymentCommand>
{
    public AddPaymentCommandValidator()
    {
        RuleFor(x => x.SaleId)
            .NotEmpty().WithMessage("Sale id is required.");

        RuleFor(x => x.Request.Method)
            .IsInEnum().WithMessage("Invalid payment method.");

        RuleFor(x => x.Request.Value)
            .GreaterThan(0).WithMessage("Value must be greater than zero.");
    }
}