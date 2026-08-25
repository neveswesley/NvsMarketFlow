using FluentValidation;
using NvsMarketFlow.Application.UseCases.Supplier.Commands;

namespace NvsMarketFlow.Application.UseCases.Supplier.Validators;

public class ActivateSupplierCommandValidator : AbstractValidator<ActivateSupplier.ActivateSupplierCommand>
{
    public ActivateSupplierCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Supplier id is required.");
    }
}