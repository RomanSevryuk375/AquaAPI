namespace Notification.Application.DTOs.Reminder;

public record ReminderUpdateRequestDto
{
    public string TaskName { get; private set; } = string.Empty;
    public int IntervalDays { get; private set; }
}
