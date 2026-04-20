using Notification.Application.DTOs.Notification;

namespace Notification.Application.Interfaces;

public interface INotificationService
{
    Task<IReadOnlyList<NotificationResponseDto>> GetAllNotificationsAsync(
        NotificationFilterDto filter, 
        int? skip, 
        int? take, 
        CancellationToken cancellationToken);

    Task<NotificationResponseDto> GetNotificationByIdAsync(
        Guid id, 
        CancellationToken cancellationToken);

    Task MarkNotificationAsReadAsync(
        Guid id,
        CancellationToken cancellationToken);
}