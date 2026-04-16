namespace Notification.Application.Interfaces;

public interface IReminderProcessor
{
    Task ProcessAsync(CancellationToken cancellationToken);
}