using BuildingBlocks.Domain.Abstractions;

namespace Notification.Application.Interfaces;

public interface IReminderBoundRequest : IUserBoundRequest
{
    public Guid ReminderId { get; }
}
