using FluentValidation;
using NvsMarketFlow.Application.UseCases.Auth.Commands;

namespace NvsMarketFlow.Application.UseCases.Auth.Validators;

public class RefreshAccessTokenCommandValidator : AbstractValidator<RefreshAccessToken.RefreshAccessTokenCommand>
{
    public RefreshAccessTokenCommandValidator()
    {
        RuleFor(x => x.Request.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.");
    }
}