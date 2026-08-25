using FluentValidation;
using NvsMarketFlow.Application.UseCases.StockMovement.Queries;

namespace NvsMarketFlow.Application.UseCases.StockMovement.Validators;

public sealed class GetAllStockMovementQueryValidator
    : AbstractValidator<GetAllStockMovement.GetAllStockMovementQuery>
{
    public GetAllStockMovementQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product id must be valid.")
            .When(x => x.ProductId.HasValue);

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User id must be valid.")
            .When(x => x.UserId.HasValue);

        RuleFor(x => x.MovementType)
            .IsInEnum().WithMessage("Invalid movement type.")
            .When(x => x.MovementType.HasValue);

        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Start date cannot be in the future.")
            .When(x => x.StartDate.HasValue);

        RuleFor(x => x.EndDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("End date cannot be in the future.")
            .When(x => x.EndDate.HasValue);

        RuleFor(x => x)
            .Must(x => !x.StartDate.HasValue ||
                       !x.EndDate.HasValue ||
                       x.StartDate <= x.EndDate)
            .WithMessage("Start date cannot be greater than end date.");
    }
}