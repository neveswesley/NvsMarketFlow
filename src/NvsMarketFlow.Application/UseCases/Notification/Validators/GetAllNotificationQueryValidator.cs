using FluentValidation;
using NvsMarketFlow.Application.UseCases.Notification.Queries;

namespace NvsMarketFlow.Application.UseCases.Notification.Validators;

public sealed class GetAllNotificationQueryValidator : AbstractValidator<GetAllNotification.GetAllNotificationQuery>
{
    public GetAllNotificationQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User id is required.");
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}