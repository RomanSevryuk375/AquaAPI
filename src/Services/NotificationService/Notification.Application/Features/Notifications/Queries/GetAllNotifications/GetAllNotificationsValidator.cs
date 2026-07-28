// Ignore Spelling: Validator

using FluentValidation;

namespace Notification.Application.Features.Notifications.Queries.GetAllNotifications;

public sealed class GetAllNotificationsValidator
    : AbstractValidator<GetAllNotificationsQuery>
{
    public GetAllNotificationsValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Level)
            .IsInEnum()
            .When(x => x.Level.HasValue);

        RuleFor(x => x.Skip)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Take)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);
    }
}
