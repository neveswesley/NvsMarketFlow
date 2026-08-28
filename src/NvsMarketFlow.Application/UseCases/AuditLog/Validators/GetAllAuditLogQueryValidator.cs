using FluentValidation;
using NvsMarketFlow.Application.UseCases.AuditLog.Queries;

namespace NvsMarketFlow.Application.UseCases.AuditLog.Validators;

public sealed class GetAllAuditLogQueryValidator
    : AbstractValidator<GetAllAuditLog.GetAllAuditLogQuery>
{
    public GetAllAuditLogQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User id must be valid.")
            .When(x => x.UserId.HasValue);

        RuleFor(x => x)
            .Must(x => !x.StartDate.HasValue ||
                       !x.EndDate.HasValue ||
                       x.StartDate <= x.EndDate)
            .WithMessage("Start date cannot be greater than end date.");
    }
}