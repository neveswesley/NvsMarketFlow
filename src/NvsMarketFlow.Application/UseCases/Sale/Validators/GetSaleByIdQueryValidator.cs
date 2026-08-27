using FluentValidation;
using NvsMarketFlow.Application.UseCases.Sale.Queries;

namespace NvsMarketFlow.Application.UseCases.Sale.Validators;

public sealed class GetSaleByIdQueryValidator
    : AbstractValidator<GetSaleById.GetSaleByIdQuery>
{
    public GetSaleByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale id is required.");
    }
}