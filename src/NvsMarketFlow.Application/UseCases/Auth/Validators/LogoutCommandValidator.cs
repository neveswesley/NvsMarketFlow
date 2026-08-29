using FluentValidation;
using NvsMarketFlow.Application.UseCases.Auth.Commands;

namespace NvsMarketFlow.Application.UseCases.Auth.Validators;

public class LogoutCommandValidator : AbstractValidator<Logout.LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(x => x.Request.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.");
    }
}