using FluentValidation;
using NvsMarketFlow.Application.UseCases.Sale.Commands;

namespace NvsMarketFlow.Application.UseCases.Sale.Validators;

public class CreateSaleCommandValidator : AbstractValidator<CreateSale.CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(x => x.Request.CashRegisterId)
            .NotEmpty().WithMessage("Cash register id is required.");
    }
}