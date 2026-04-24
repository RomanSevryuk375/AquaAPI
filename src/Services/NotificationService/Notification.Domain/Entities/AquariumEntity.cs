using Contracts.Abstractions;

namespace Notification.Domain.Entities;

public class AquariumEntity : IEntity
{
    private AquariumEntity(
        Guid id,
        Guid userId,
        string name,
        DateTime createdAt)
    {
        Id = id;
        UserId = userId;
        Name = name;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    public static (AquariumEntity? aquarium, List<string> errors) Create(
        Guid id,
        Guid userId,
        string name,
        DateTime createdAt)
    {
        var errors = new List<string>();

        if (id == Guid.Empty)
        {
            errors.Add("id must not be empty.");
        }

        if (userId == Guid.Empty)
        {
            errors.Add("userId must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            errors.Add("name must not be empty.");
        }

        if (errors.Count > 0)
        {
            return (null, errors);
        }

        var aquarium = new AquariumEntity(
            id,
            userId,
            name,
            createdAt);

        return (aquarium, errors);
    }

    public string? SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return("name must not be empty.");
        }

        if (Name == name)
        {
            return null;
        }

        Name = name;

        return null;
    }
}
