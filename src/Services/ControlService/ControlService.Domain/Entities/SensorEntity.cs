using Contracts.Abstractions;
using Contracts.Enums;

namespace Control.Domain.Entities;

public class SensorEntity : IEntity
{
    private SensorEntity(
        Guid id, 
        Guid aquariumId, 
        SensorStateEnum state,
        SensorTypeEnum type, 
        DateTime createdAt)
    {
        Id = id;
        AquariumId = aquariumId;
        State = state;
        Type = type;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public Guid AquariumId { get; private set; }
    public SensorStateEnum State { get; private set; }
    public SensorTypeEnum Type { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static (SensorEntity? sensor, List<string> errors) Create(
        Guid id,
        Guid aquariumId,
        SensorStateEnum state,
        SensorTypeEnum type,
        DateTime createdAt)
    {
        var errors = new List<string>();

        if (id == Guid.Empty)
        {
            errors.Add("id must not be empty.");
        }

        if (aquariumId == Guid.Empty)
        {
            errors.Add("aquariumId must not be empty.");
        }

        if (errors.Count > 0)
        {
            return (null, errors);
        }

        var sensor = new SensorEntity(
            id,
            aquariumId,
            state,
            type,
            createdAt);

        return (sensor, errors);
    }

    public void SetState(SensorStateEnum state)
    {
        if (State == state)
        {
            return;
        }

        State = state;
    }

    public void SetType(SensorTypeEnum type)
    {
        if (Type == type)
        {
            return;
        }

        Type = type;
    }
}
