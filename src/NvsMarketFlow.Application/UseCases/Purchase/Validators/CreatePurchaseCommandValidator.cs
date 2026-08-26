using FluentValidation;
using NvsMarketFlow.Application.UseCases.Purchase.Commands;

namespace NvsMarketFlow.Application.UseCases.Purchase.Validators;

public class CreatePurchaseCommandValidator : AbstractValidator<CreatePurchase.CreatePurchaseCommand>
{
    public CreatePurchaseCommandValidator()
    {
        RuleFor(x => x.Request.SupplierId)
            .NotEmpty().WithMessage("Supplier id is required.");

        RuleFor(x => x.Request.InvoiceNumber)
            .NotEmpty().WithMessage("Invoice number is required.")
            .MaximumLength(50).WithMessage("Invoice number must be at most 50 characters long.");
    }
}