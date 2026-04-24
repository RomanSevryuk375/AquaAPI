using Contracts.Abstractions;
using Contracts.Enums;

namespace Control.Domain.Entities;

public class RelayEntity : IEntity
{
    private RelayEntity(
        Guid id,
        Guid aquariumId,
        RelayPurposeEnum purpose,
        bool isManual,
        bool isActive,
        DateTime createdAt)
    {
        Id = id;
        AquariumId = aquariumId;
        Purpose = purpose;
        IsManual = isManual;
        IsActive = isActive;
        CreatedAt = createdAt;
    }
    public Guid Id { get; private set; }
    public Guid AquariumId { get; private set; }
    public RelayPurposeEnum Purpose { get; private set; }
    public bool IsManual { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static (RelayEntity? relay, List<string> errors) Create(
        Guid id,
        Guid aquariumId,
        RelayPurposeEnum purpose,
        bool isManual,
        bool isActive,
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

        var relay = new RelayEntity(
            id,
            aquariumId,
            purpose,
            isManual,
            isActive,
            createdAt);

        return (relay, errors);
    }

    public void SetPurpose(RelayPurposeEnum purpose)
    {
        if (Purpose == purpose)
        {
            return;
        }

        Purpose = purpose;
    }

    public void SetMode(bool mode)
    {
        IsManual = mode;
    }

    public void SetState(bool state)
    {
        IsActive = state;
    }
}
