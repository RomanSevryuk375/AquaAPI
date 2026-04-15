using Contracts.Enums;
using Notification.Domain.Interfaces;

namespace Notification.Domain.Entities;

public class NotificationEntity : IEntity
{
    private NotificationEntity(
        Guid id, 
        Guid userId, 
        Guid aquariumId, 
        NotificationLevelEnum level, 
        string message, 
        DateTime createdAt,
        bool isPublished,
        DateTime? publishedAt)
    {
        Id = id;
        UserId = userId;
        AquariumId = aquariumId;
        Level = level;
        Message = message;
        IsRead = false;
        CreatedAt = createdAt;
        IsPublished = isPublished;
        PublishedAt = publishedAt;
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid AquariumId { get; private set; }
    public NotificationLevelEnum Level { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public bool IsRead { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsPublished { get; private set; }
    public DateTime? PublishedAt { get; private set; }

    public static (NotificationEntity? notification, List<string> errors) Create(
        Guid userId, 
        Guid aquariumId, 
        NotificationLevelEnum level, 
        string message)
    {
        var errors = new List<string>();

        if (userId == Guid.Empty)
        {
            errors.Add("userId must not be empty.");
        }

        if (aquariumId == Guid.Empty)
        {
            errors.Add("aquariumId must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            errors.Add("message must not be empty.");
        }

        if (errors.Count > 0)
        {
            return (null, errors);
        }

        var notification = new NotificationEntity(
            Guid.NewGuid(),
            userId,
            aquariumId,
            level,
            message,
            DateTime.UtcNow,
            false,
            null);

        return (notification, errors);
    }

    public void MarkAsRead()
    {
        IsRead = true;
    }

    public void MarkAsPublished()
    {
        IsPublished = true;
        PublishedAt = DateTime.UtcNow;
    }
}