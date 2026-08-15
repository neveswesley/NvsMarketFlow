using FluentValidation;
using NvsMarketFlow.Application.UseCases.User.Commands;

namespace NvsMarketFlow.Application.UseCases.User.Validators;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUser.DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product id is required.");
    }
}