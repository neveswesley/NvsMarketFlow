using FluentValidation;
using NvsMarketFlow.Application.UseCases.Supplier.Queries;

namespace NvsMarketFlow.Application.UseCases.Supplier.Validators;

public sealed class GetAllSupplierQueryValidator
    : AbstractValidator<GetAllSupplier.GetAllSupplierQuery>
{
    public GetAllSupplierQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid status.")
            .When(x => x.Status.HasValue);
    }
}