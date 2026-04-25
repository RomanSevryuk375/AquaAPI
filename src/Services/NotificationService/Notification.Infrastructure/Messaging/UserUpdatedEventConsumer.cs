using Contracts.Events.UserEvents;
using MassTransit;
using Notification.Application.Interfaces;

namespace Notification.Infrastructure.Messaging;

public class UserUpdatedEventConsumer(
    IUserServiceFromEvent userService) : IConsumer<UserUpdatedEvent>
{
    public async Task Consume(ConsumeContext<UserUpdatedEvent> context)
    {
        await userService
            .UpdateUserFromEventAsync(context.Message, context.CancellationToken);
    }
}
