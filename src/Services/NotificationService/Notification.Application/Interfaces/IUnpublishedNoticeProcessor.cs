namespace Notification.Application.Services;

public interface IUnpublishedNoticeProcessor
{
    Task ProcessAsync(CancellationToken cancellationToken);
}