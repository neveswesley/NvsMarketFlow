using FluentValidation;
using NvsMarketFlow.Application.UseCases.User.Query;

namespace NvsMarketFlow.Application.UseCases.User.Validators;

public class GetAllUsersQueryValidator : AbstractValidator<GetAllUsers.GetAllUsersQuery>
{
    public GetAllUsersQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be greater than zero.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than zero.")
            .LessThanOrEqualTo(100).WithMessage("Page size must be at most 100.");
    }
}