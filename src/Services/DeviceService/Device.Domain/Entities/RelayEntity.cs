using Contracts.Abstractions;
using Contracts.Enums;

namespace Device.Domain.Entities;

public sealed class RelayEntity : IEntity
{
    private RelayEntity(
        Guid id,
        Guid controllerId,
        string hardwarePin,
        RelayPurposeEnum purpose,
        bool isActive,
        bool isManual,
        DateTime createdAt)
    {
        Id = id;
        ControllerId = controllerId;
        HardwarePin = hardwarePin;
        Purpose = purpose;
        IsActive = isActive;
        IsManual = isManual;
        CreatedAt = createdAt;
    }
    
    public Guid Id { get; private set; }
    public Guid ControllerId { get; private set; }
    public string HardwarePin { get; private set; } = string.Empty;
    public RelayPurposeEnum Purpose { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsManual { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static (RelayEntity? relay, List<string>? errors) Create (
        Guid controllerId,
        string hardwarePin,
        RelayPurposeEnum purpose,
        bool isActive,
        bool isManual)
    {
        var errors = new List<string>();

        if (controllerId == Guid.Empty)
        {
            errors.Add("controllerId must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(hardwarePin))
        {
            errors.Add("hardwarePin must not be empty.");
        }

        if (errors.Count > 0)
        {
            return (null, errors);
        }

        var relay = new RelayEntity(
            Guid.NewGuid(),
            controllerId,
            hardwarePin.Trim(),
            purpose,
            isActive,
            isManual,
            DateTime.UtcNow);

        return (relay, errors);
    }

    public List<string>? Update(
        Guid controllerId,
        string hardwarePin,
        RelayPurposeEnum purpose)
    {
        var errors = new List<string>();

        if (controllerId == Guid.Empty)
        {
            errors.Add("controllerId must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(hardwarePin))
        {
            errors.Add("hardwarePin must not be empty.");
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        ControllerId = controllerId;
        HardwarePin = hardwarePin.Trim();
        Purpose = purpose;

        return null;
    }

    public void ToggleState()
    {
        IsActive = !IsActive;
    }

    public void SetState(bool state)
    {
        if (IsActive == state)
        {
            return;
        }

        IsActive = state;
    }

    public void ToggleMode()
    {
        IsManual = !IsManual;
    }
}
