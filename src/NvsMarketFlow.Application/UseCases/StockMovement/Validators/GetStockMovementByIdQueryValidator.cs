using FluentValidation;
using NvsMarketFlow.Application.UseCases.StockMovement.Queries;

namespace NvsMarketFlow.Application.UseCases.StockMovement.Validators;

public sealed class GetStockMovementByIdQueryValidator
    : AbstractValidator<GetStockMovementById.GetStockMovementByIdQuery>
{
    public GetStockMovementByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("StockMovement id is required.");
    }
}