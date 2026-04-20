namespace Notification.Application.DTOs.Reminder;

public record ReminderResponseDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid AquariumId { get; init; }
    public string TaskName { get; init; } = string.Empty;
    public int IntervalDays { get; init; }
    public DateTime? LastDoneAt { get; init; }
    public DateTime? LastNotifiedAt { get; init; }
    public DateTime NextDueAt { get; init; }
    public DateTime CreatedAt { get; init; }
}
