using Device.Application.Constants;
using Device.Domain.Events.RelayEvents;
using ZiggyCreatures.Caching.Fusion;

namespace Device.Application.Handlers;

public sealed class RelayCacheInvalidationHandler(IFusionCache cache) :
    INotificationHandler<RelayUpdatedDomainEvent>,
    INotificationHandler<SetRelayPowerSensorDomainEvent>,
    INotificationHandler<RelayStateChangedDomainEvent>,
    INotificationHandler<RelayModeChangedDomainEvent>,
    INotificationHandler<RelayDeletedDomainEvent>
{
    public async Task Handle(RelayUpdatedDomainEvent notification, CancellationToken cancellationToken) => 
        await cache.RemoveAsync(CacheKeys.Relay(notification.UserId, notification.RelayId), token: cancellationToken);

    public async Task Handle(SetRelayPowerSensorDomainEvent notification, CancellationToken cancellationToken) =>
        await cache.RemoveAsync(CacheKeys.Relay(notification.UserId, notification.RelayId), token: cancellationToken);

    public async Task Handle(RelayStateChangedDomainEvent notification, CancellationToken cancellationToken) =>
        await cache.RemoveAsync(CacheKeys.Relay(notification.UserId, notification.RelayId), token: cancellationToken);

    public async Task Handle(RelayModeChangedDomainEvent notification, CancellationToken cancellationToken) =>
        await cache.RemoveAsync(CacheKeys.Relay(notification.UserId, notification.RelayId), token: cancellationToken);

    public async Task Handle(RelayDeletedDomainEvent notification, CancellationToken cancellationToken) => 
        await cache.RemoveAsync(CacheKeys.Relay(notification.UserId, notification.RelayId), token: cancellationToken);
}
