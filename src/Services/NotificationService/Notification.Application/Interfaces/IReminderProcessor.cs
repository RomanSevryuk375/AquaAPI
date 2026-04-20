namespace Notification.Application.Interfaces;

public interface IReminderProcessor
{
    Task CheckAsync(CancellationToken cancellationToken);
}