using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using Notification.Application.Constants;
using Notification.Application.Features.Reminders.Queries.Shared;

namespace Notification.Application.Features.Reminders.Queries.GetReminderById;

public sealed record GetReminderByIdQuery
    : ICachedQuery<Result<ReminderDto>>
{
    public Guid ReminderId { get; init; }
    public Guid UserId { get; init; }

    public string CacheKey => CacheKeys.Reminder(UserId, ReminderId);
    public TimeSpan? Expiration => TimeSpan.FromMinutes(2);
}
