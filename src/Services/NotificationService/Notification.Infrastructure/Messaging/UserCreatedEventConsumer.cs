using Contracts.Events.UserEvents;
using MassTransit;
using Notification.Application.Interfaces;

namespace Notification.Infrastructure.Messaging;

public class UserCreatedEventConsumer(
    IUserServiceFromEvent service) : IConsumer<UserCreatedEvent>
{
    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        await service.CreateUserFromEventAsync(
            context.Message, context.CancellationToken);
    }
}
