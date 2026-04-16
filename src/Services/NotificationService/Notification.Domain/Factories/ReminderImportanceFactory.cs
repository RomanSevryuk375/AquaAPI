using Contracts.Enums;

namespace Notification.Domain.Factories;

public static class ReminderImportanceFactory
{
    public static NotificationLevelEnum Evaluate (DateTime nextDueAt)
    {
        var now = DateTime.UtcNow;
        var timeDiff = nextDueAt - now;

        if (timeDiff.Hours < 24)
        {
            return NotificationLevelEnum.Info;
        }

        if (timeDiff.Hours <= 0)
        {
            return NotificationLevelEnum.Warning;
        }

        if (timeDiff.Hours <= -24)
        {
            return NotificationLevelEnum.Critical;
        }

        return NotificationLevelEnum.Info;
    }
}
