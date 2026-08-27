using FluentValidation;
using NvsMarketFlow.Application.UseCases.Sale.Commands;

namespace NvsMarketFlow.Application.UseCases.Sale.Validators;

public class CancelSaleCommandValidator : AbstractValidator<CancelSale.CancelSaleCommand>
{
    public CancelSaleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale id is required.");
    }
}