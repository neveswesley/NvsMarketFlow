using FluentValidation;
using NvsMarketFlow.Application.UseCases.Auth.Commands;

namespace NvsMarketFlow.Application.UseCases.Auth.Validators;

public class LoginCommandValidator : AbstractValidator<Login.LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Request.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be valid.");

        RuleFor(x => x.Request.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}