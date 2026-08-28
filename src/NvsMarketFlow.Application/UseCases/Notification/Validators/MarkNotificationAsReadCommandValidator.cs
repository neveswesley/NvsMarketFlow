using FluentValidation;
using NvsMarketFlow.Application.UseCases.Notification.Commands;

namespace NvsMarketFlow.Application.UseCases.Notification.Validators;

public class MarkNotificationAsReadCommandValidator : AbstractValidator<MarkNotificationAsRead.MarkNotificationAsReadCommand>
{
    public MarkNotificationAsReadCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Notification id is required.");
    }
}