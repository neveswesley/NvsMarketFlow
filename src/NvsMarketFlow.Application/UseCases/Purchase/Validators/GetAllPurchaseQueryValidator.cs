using FluentValidation;
using NvsMarketFlow.Application.UseCases.Purchase.Queries;

namespace NvsMarketFlow.Application.UseCases.Purchase.Validators;

public sealed class GetAllPurchaseQueryValidator
    : AbstractValidator<GetAllPurchase.GetAllPurchaseQuery>
{
    public GetAllPurchaseQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.SupplierId)
            .NotEmpty().WithMessage("Supplier id must be valid.")
            .When(x => x.SupplierId.HasValue);

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid status.")
            .When(x => x.Status.HasValue);

        RuleFor(x => x)
            .Must(x => !x.StartDate.HasValue ||
                       !x.EndDate.HasValue ||
                       x.StartDate <= x.EndDate)
            .WithMessage("Start date cannot be greater than end date.");
    }
}