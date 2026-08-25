using FluentValidation;
using NvsMarketFlow.Application.UseCases.Supplier.Queries;

namespace NvsMarketFlow.Application.UseCases.Supplier.Validators;

public sealed class GetSupplierByIdQueryValidator
    : AbstractValidator<GetSupplierById.GetSupplierByIdQuery>
{
    public GetSupplierByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Supplier id is required.");
    }
}