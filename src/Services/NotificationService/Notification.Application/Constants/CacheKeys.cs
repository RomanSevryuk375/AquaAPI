namespace Notification.Application.Constants;

public static class CacheKeys
{
    public static string Reminder(Guid userId, Guid reminderId) => $"reminder:{userId}:{reminderId}";
    public static string Notification(Guid userId, Guid notificationId) => $"notification:{userId}:{notificationId}";
    public static string MaintenanceLog(Guid userId, Guid logId) => $"maintenance-log:{userId}:{logId}";
}
