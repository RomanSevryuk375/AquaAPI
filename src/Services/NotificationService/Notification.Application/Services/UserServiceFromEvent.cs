using Contracts.Events.UserEvents;
using Contracts.Exceptions;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;
using Notification.Domain.Interfaces;

namespace Notification.Application.Services;

public class UserServiceFromEvent(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IUserServiceFromEvent
{
    public async Task CreateUserFromEventAsync(
        UserCreatedEvent userCreated,
        CancellationToken cancellationToken)
    {
        var existingUser = await userRepository
            .GetByIdAsync(userCreated.UserId, cancellationToken);

        if (existingUser is not null)
        {
            return;
        }

        var (user, errors) = UserEntity.Create(
            userCreated.UserId,
            userCreated.Email,
            userCreated.PhoneNumber,
            false,
            false,
            null,
            false,
            userCreated.CreatedAt);

        if (user is null)
        {
            throw new DomainValidationException(
                $"Failed to create {nameof(UserEntity)}: {string.Join(", ", errors)}");
        }

        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
