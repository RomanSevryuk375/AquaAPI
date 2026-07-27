using AutoMapper;
using BuildingBlocks.IntegrationEvents.Events.Users;
using IdentityService.Domain.Events;
using MassTransit;
using MediatR;

namespace IdentityService.Application.Handlers;

public sealed class UserUpdatedEventHandler(
    IPublishEndpoint publishEndpoint,
    IMapper mapper) : INotificationHandler<UserUpdatedDomainEvent>
{
    public async Task Handle(UserUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        UserUpdatedEvent integrationEvent = mapper.Map<UserUpdatedEvent>(notification);

        await publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}
