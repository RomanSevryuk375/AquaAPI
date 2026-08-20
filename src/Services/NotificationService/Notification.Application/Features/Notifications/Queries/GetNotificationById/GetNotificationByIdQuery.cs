using BuildingBlocks.Domain.Abstractions;
using BuildingBlocks.Domain.Results;
using Notification.Application.Constants;
using Notification.Application.Features.Notifications.Queries.Shared;

namespace Notification.Application.Features.Notifications.Queries.GetNotificationById;

public sealed record GetNotificationByIdQuery
    : ICachedQuery<Result<NotificationDto>>
{
    public Guid NotificationId { get; init; }
    public Guid UserId { get; init; }

    public string CacheKey => CacheKeys.Notification(UserId, NotificationId);
    public TimeSpan? Expiration => TimeSpan.FromMinutes(2);
}
