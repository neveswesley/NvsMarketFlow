using FluentValidation;
using NvsMarketFlow.Application.UseCases.Sale.Commands;

namespace NvsMarketFlow.Application.UseCases.Sale.Validators;

public class RemoveSaleItemCommandValidator : AbstractValidator<RemoveSaleItem.RemoveSaleItemCommand>
{
    public RemoveSaleItemCommandValidator()
    {
        RuleFor(x => x.SaleId)
            .NotEmpty().WithMessage("Sale id is required.");

        RuleFor(x => x.ItemId)
            .NotEmpty().WithMessage("Item id is required.");
    }
}