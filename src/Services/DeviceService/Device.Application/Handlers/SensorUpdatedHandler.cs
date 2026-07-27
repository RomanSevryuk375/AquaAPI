using BuildingBlocks.IntegrationEvents.Events.Sensors;
using Device.Domain.Events.SensorEvents;
using MassTransit;

namespace Device.Application.Handlers;

internal sealed class SensorUpdatedHandler(
    IPublishEndpoint publishEndpoint,
    IMapper mapper) : INotificationHandler<SensorUpdatedDomainEvent>
{
    public async Task Handle(
        SensorUpdatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        await publishEndpoint.Publish(
            mapper.Map<SensorUpdatedEvent>(notification), cancellationToken);
    }
}
