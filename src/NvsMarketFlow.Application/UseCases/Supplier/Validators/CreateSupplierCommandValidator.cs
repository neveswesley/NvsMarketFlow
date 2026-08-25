using FluentValidation;
using NvsMarketFlow.Application.UseCases.Supplier.Commands;

namespace NvsMarketFlow.Application.UseCases.Supplier.Validators;

public class CreateSupplierCommandValidator : AbstractValidator<CreateSupplier.CreateSupplierCommand>
{
    public CreateSupplierCommandValidator()
    {
        RuleFor(x => x.Request.CorporateName)
            .NotEmpty().WithMessage("Corporate name is required.")
            .MinimumLength(3).WithMessage("Corporate name must be at least 3 characters long.")
            .MaximumLength(200).WithMessage("Corporate name must be at most 200 characters long.");

        RuleFor(x => x.Request.FantasyName)
            .NotEmpty().WithMessage("Fantasy name is required.")
            .MinimumLength(3).WithMessage("Fantasy name must be at least 3 characters long.")
            .MaximumLength(150).WithMessage("Fantasy name must be at most 150 characters long.");

        RuleFor(x => x.Request.CNPJ)
            .NotEmpty().WithMessage("CNPJ is required.")
            .Matches(@"^\d{2}\.?\d{3}\.?\d{3}\/?\d{4}-?\d{2}$")
            .WithMessage("CNPJ must be a valid format.");

        RuleFor(x => x.Request.Phone)
            .NotEmpty().WithMessage("Phone is required.")
            .MaximumLength(20).WithMessage("Phone must be at most 20 characters long.");

        RuleFor(x => x.Request.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.")
            .MaximumLength(150).WithMessage("Email must be at most 150 characters long.");

        RuleFor(x => x.Request.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(250).WithMessage("Address must be at most 250 characters long.");

        RuleFor(x => x.Request.Status)
            .IsInEnum().WithMessage("Invalid status.");
    }
}