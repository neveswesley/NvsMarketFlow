using FluentValidation;
using NvsMarketFlow.Application.UseCases.Supplier.Commands;

namespace NvsMarketFlow.Application.UseCases.Supplier.Validators;

public class DeactivateSupplierCommandValidator : AbstractValidator<DeactivateSupplier.DeactivateSupplierCommand>
{
    public DeactivateSupplierCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Supplier id is required.");
    }
}