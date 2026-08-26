using FluentValidation;
using NvsMarketFlow.Application.UseCases.StockMovement.Commands;
using NvsMarketFlow.Domain.Enums;

namespace NvsMarketFlow.Application.UseCases.StockMovement.Validators;

public class CreateStockMovementCommandValidator : AbstractValidator<CreateStockMovement.CreateStockMovementCommand>
{
    public CreateStockMovementCommandValidator()
    {
        RuleFor(x => x.Request)
            .NotNull()
            .WithMessage("The request cannot be null.");

        When(x => x.Request is not null, () =>
        {
            RuleFor(x => x.Request.ProductId)
                .NotEmpty()
                .WithMessage("The product is required.");

            RuleFor(x => x.Request.UserId)
                .NotEmpty()
                .WithMessage("The user is required.");

            RuleFor(x => x.Request.MovementType)
                .IsInEnum()
                .WithMessage("Invalid movement type.");

            RuleFor(x => x.Request.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero.");

            RuleFor(x => x.Request.Reason)
                .NotEmpty()
                .WithMessage("The movement reason is required.")
                .MaximumLength(500)
                .WithMessage("The reason cannot exceed 500 characters.");

            RuleFor(x => x.Request.IsIncrease)
                .NotNull()
                .WithMessage("You must specify whether the adjustment increases or decreases stock.")
                .When(x => x.Request.MovementType is MovementType.Inventory or MovementType.ManualAdjustment);
        });
    }
}