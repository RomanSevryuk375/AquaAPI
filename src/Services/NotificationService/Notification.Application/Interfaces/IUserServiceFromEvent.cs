using Contracts.Events.UserEvents;

namespace Notification.Application.Interfaces;

public interface IUserServiceFromEvent
{
    Task CreateUserFromEventAsync(
        UserCreatedEvent userCreated, 
        CancellationToken cancellationToken);

    Task UpdateUserFromEventAsync(
        UserUpdatedEvent user,
        CancellationToken cancellationToken);
}