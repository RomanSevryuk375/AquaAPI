using BuildingBlocks.Domain.Abstractions;
using Notification.Application.Interfaces;

namespace Notification.Application.Features.Reminders.Commands.DeleteReminder;

public sealed record DeleteReminderCommand : ICommand, IReminderBoundRequest
{
    public Guid UserId { get; init; }
    public Guid ReminderId { get; init; }
}
