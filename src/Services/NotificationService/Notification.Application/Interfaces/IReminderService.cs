using Notification.Application.DTOs.Reminder;

namespace Notification.Application.Interfaces;

public interface IReminderService
{
    Task<IReadOnlyList<ReminderResponseDto>> GetAllRemindersAsync(
        ReminderFilterDto filter,
        int? skip,
        int? take,
        CancellationToken cancellationToken);

    Task<ReminderResponseDto> GetReminderByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<Guid> AddReminderAsync(
        ReminderRequestDto request,
        CancellationToken cancellationToken);

    Task UpdateReminderAsync(
        Guid id,
        ReminderUpdateRequestDto request,
        CancellationToken cancellationToken);

    Task ReminderCompleteTaskAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task DeleteReminderAsync(
        Guid id,
        CancellationToken cancellationToken);
}
