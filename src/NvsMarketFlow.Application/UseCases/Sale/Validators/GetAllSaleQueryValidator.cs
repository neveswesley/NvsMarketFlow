using FluentValidation;
using NvsMarketFlow.Application.UseCases.Sale.Queries;

namespace NvsMarketFlow.Application.UseCases.Sale.Validators;

public sealed class GetAllSaleQueryValidator
    : AbstractValidator<GetAllSale.GetAllSaleQuery>
{
    public GetAllSaleQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.CashRegisterId)
            .NotEmpty().WithMessage("Cash register id must be valid.")
            .When(x => x.CashRegisterId.HasValue);

        RuleFor(x => x.SellerId)
            .NotEmpty().WithMessage("Seller id must be valid.")
            .When(x => x.SellerId.HasValue);

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