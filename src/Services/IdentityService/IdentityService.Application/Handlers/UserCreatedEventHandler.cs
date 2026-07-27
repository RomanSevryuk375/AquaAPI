using AutoMapper;
using BuildingBlocks.IntegrationEvents.Events.Users;
using IdentityService.Domain.Events;
using MassTransit;
using MediatR;

namespace IdentityService.Application.Handlers;

public sealed class UserCreatedEventHandler(
    IPublishEndpoint publishEndpoint,
    IMapper mapper) : INotificationHandler<UserCreatedDomainEvent>
{
    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        UserCreatedEvent integrationEvent = mapper.Map<UserCreatedEvent>(notification);

        await publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}
