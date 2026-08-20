using Device.Application.Constants;
using Device.Domain.Events.ControllerEvents;
using ZiggyCreatures.Caching.Fusion;

namespace Device.Application.Handlers;

public sealed class ControllerCacheInvalidationHandler(IFusionCache cache) :
    INotificationHandler<ControllerNotOnlineDomainEvent>
{
    public async Task Handle(ControllerNotOnlineDomainEvent notification, CancellationToken cancellationToken) =>
        await cache.RemoveAsync(CacheKeys.Controller(notification.UserId, notification.ControllerId), token: cancellationToken);
}
