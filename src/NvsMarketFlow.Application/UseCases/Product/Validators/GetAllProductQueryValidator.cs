using FluentValidation;
using NvsMarketFlow.Application.UseCases.Product.Queries;

namespace NvsMarketFlow.Application.UseCases.Product.Validators;

public sealed class GetAllProductQueryValidator
    : AbstractValidator<GetAllProduct.GetAllProductQuery>
{
    public GetAllProductQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MinPrice.HasValue);

        RuleFor(x => x.MaxPrice)
            .GreaterThanOrEqualTo(0)
            .When(x => x.MaxPrice.HasValue);

        RuleFor(x => x)
            .Must(x => !x.MinPrice.HasValue ||
                       !x.MaxPrice.HasValue ||
                       x.MinPrice <= x.MaxPrice)
            .WithMessage("Minimum price cannot be greater than maximum price.");
    }
}