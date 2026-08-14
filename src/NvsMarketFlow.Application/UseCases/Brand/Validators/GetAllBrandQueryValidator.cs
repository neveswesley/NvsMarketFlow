using FluentValidation;
using NvsMarketFlow.Application.UseCases.Brand.Queries;

namespace NvsMarketFlow.Application.UseCases.Brand.Validators;

public class GetAllBrandQueryValidator : AbstractValidator<GetAllBrands.GetAllBrandsQuery>
{
    public GetAllBrandQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);
    }
}