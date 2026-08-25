using FluentValidation;
using NvsMarketFlow.Application.UseCases.User.Commands;

namespace NvsMarketFlow.Application.UseCases.User.Validators;

public class UpdateNameCommandValidator : AbstractValidator<UpdateName.UpdateNameCommand>
{
    public UpdateNameCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("The id cannot be null")
            .NotEmpty().WithMessage("The id cannot be empty");

        RuleFor(x => x.Request.NewName)
            .NotEmpty().WithMessage("The new name cannot be empty")
            .MinimumLength(2).WithMessage("The new name must be at least 2 characters long")
            .MaximumLength(100).WithMessage("The new name must be less than 100 characters long");


    }
}