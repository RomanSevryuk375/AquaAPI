namespace Notification.Application.DTOs.Reminder;

public record ReminderRequestDto
{
    public Guid UserId { get; init; }
    public Guid AquariumId { get; init; }
    public string TaskName { get; init; } = string.Empty;
    public int IntervalDays { get; init; }
}
