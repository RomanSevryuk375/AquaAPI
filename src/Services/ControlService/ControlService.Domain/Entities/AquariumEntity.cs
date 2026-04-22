using Control.Domain.Interfaces;

namespace Control.Domain.Entities;

public class AquariumEntity : IEntity
{
    private AquariumEntity(
        Guid id, 
        Guid userId,
        string name, 
        Guid controllerId, 
        DateTime createdAt)
    {
        Id = id;
        Name = name; 
        ControllerId = controllerId; 
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public Guid ControllerId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static (AquariumEntity? aquarium, List<string> errors) Create(
        Guid userId,
        string name,
        Guid controllerId)
    {
        var errors = new List<string>();

        if (userId == Guid.Empty)
        {
            errors.Add("userId must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            errors.Add("name must not be empty.");
        }

        if (controllerId == Guid.Empty)
        {
            errors.Add("controllerId must not be empty.");
        }

        if (errors.Count > 0)
        {
            return (null, errors);
        }

        var aquarium = new AquariumEntity(
            Guid.NewGuid(),
            userId,
            name.Trim(),
            controllerId,
            DateTime.UtcNow);

        return (aquarium, errors);
    }

    public string? SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
           return("name must not be empty.");
        }

        Name = name;

        return null;
    }
}
