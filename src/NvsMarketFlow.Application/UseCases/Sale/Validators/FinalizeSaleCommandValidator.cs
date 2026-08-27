using FluentValidation;
using NvsMarketFlow.Application.UseCases.Sale.Commands;

namespace NvsMarketFlow.Application.UseCases.Sale.Validators;

public class FinalizeSaleCommandValidator : AbstractValidator<FinalizeSale.FinalizeSaleCommand>
{
    public FinalizeSaleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale id is required.");
    }
}