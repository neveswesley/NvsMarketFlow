using FluentValidation;
using NvsMarketFlow.Application.UseCases.User.Commands;

namespace NvsMarketFlow.Application.UseCases.User.Validators;

public class UpdateEmailCommandValidator : AbstractValidator<UpdateEmail.UpdateEmailCommand>
{
    public UpdateEmailCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("UserId is required");

        RuleFor(x => x.Request.NewEmail)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email");
    }
}