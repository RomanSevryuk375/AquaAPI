using Device.Application.Constants;
using Device.Domain.Events.SensorEvents;
using ZiggyCreatures.Caching.Fusion;

namespace Device.Application.Handlers;

public sealed class SensorCacheInvalidationHandler(IFusionCache cache) :
    INotificationHandler<SensorCreatedDomainEvent>,
    INotificationHandler<SensorUpdatedDomainEvent>,
    INotificationHandler<SensorStateChangedDomainEvent>,
    INotificationHandler<SensorDeletedDomainEvent>
{
    public async Task Handle(SensorCreatedDomainEvent notification, CancellationToken cancellationToken) =>
        await cache.RemoveAsync(CacheKeys.Sensor(notification.UserId, notification.SensorId), token: cancellationToken);

    public async Task Handle(SensorUpdatedDomainEvent notification, CancellationToken cancellationToken) =>
        await cache.RemoveAsync(CacheKeys.Sensor(notification.UserId, notification.SensorId), token: cancellationToken);

    public async Task Handle(SensorStateChangedDomainEvent notification, CancellationToken cancellationToken) =>
        await cache.RemoveAsync(CacheKeys.Sensor(notification.UserId, notification.SensorId), token: cancellationToken);

    public async Task Handle(SensorDeletedDomainEvent notification, CancellationToken cancellationToken) =>
        await cache.RemoveAsync(CacheKeys.Sensor(notification.UserId, notification.SensorId), token: cancellationToken);
}
