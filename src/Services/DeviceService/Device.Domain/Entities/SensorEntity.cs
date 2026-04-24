using Contracts.Abstractions;
using Contracts.Enums;

namespace Device.Domain.Entities;

public sealed class SensorEntity : IEntity
{
    private SensorEntity(
        Guid id,
        Guid controllerId,
        string hardwarePin,
        SensorTypeEnum type,
        SensorStateEnum state,
        string unit,
        DateTime createdAt)
    {
        Id = id;
        ControllerId = controllerId;
        HardwarePin = hardwarePin;
        Type = type;
        State = state;
        Unit = unit;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public Guid ControllerId { get; private set; }
    public string HardwarePin { get; private set; } = string.Empty;
    public SensorTypeEnum Type { get; private set; }
    public SensorStateEnum State { get; private set; }
    public string Unit { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static (SensorEntity? sensor, List<string> errors) Create(
        Guid controllerId,
        string hardwarePin,
        SensorTypeEnum type,
        string unit)
    {
        var errors = new List<string>();

        if (controllerId == Guid.Empty)
        {
            errors.Add("controllerId must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(unit))
        {
            errors.Add("unit must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(hardwarePin))
        {
            errors.Add("hardwarePin must not be empty.");
        }

        if (errors.Count > 0)
        {
            return (null, errors);
        }

        var sensor = new SensorEntity(
            Guid.NewGuid(),
            controllerId,
            hardwarePin.Trim(),
            type,
            SensorStateEnum.NoData,
            unit.Trim(),
            DateTime.UtcNow);

        return (sensor, errors);
    }

    public List<string>? Update(
        string hardwarePin,
        Guid controllerId,
        SensorTypeEnum type,
        string unit)
    {
        var errors = new List<string>();

        if (controllerId == Guid.Empty)
        {
            errors.Add("controllerId must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(unit))
        {
            errors.Add("unit must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(hardwarePin))
        {
            errors.Add("hardwarePin must not be empty.");
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        HardwarePin = hardwarePin.Trim();
        ControllerId = controllerId;
        Type = type;
        Unit = unit.Trim();

        return null;
    }

    public void SetState(SensorStateEnum state)
    {
        if (State == state)
        {
            return;
        }

        State = state;
    }
}