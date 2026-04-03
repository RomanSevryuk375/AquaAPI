using Contracts.Enums;
using Telemetry.Domain.Interfaces;

namespace Telemetry.Domain.Entities;

public sealed class SensorEntity : IEntity
{
    private SensorEntity(
        Guid id,
        Guid controllerId,
        SensorTypeEnum type,
        string unit,
        double lastValue,
        DateTime updatedAt,
        DateTime createdAt)
    {
        Id = id;
        ControllerId = controllerId;
        Type = type;
        Unit = unit;
        LastValue = lastValue;
        UpdatedAt = updatedAt;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public Guid ControllerId { get; private set; }
    public SensorTypeEnum Type { get; private set; }
    public string Unit { get; private set; }
    public double LastValue { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; } 

    public static (SensorEntity? sensor, List<string> errors) Create(
        Guid id,
        Guid controllerId,
        SensorTypeEnum type,
        string unit,
        double lastValue,
        DateTime updatedAt,
        DateTime createdAt)
    {
        var errors = new List<string>();

        if (id == Guid.Empty)
        {
            errors.Add("Id must not be empty.");
        }

        if (controllerId == Guid.Empty)
        {
            errors.Add("Controller id must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(unit))
        {
            errors.Add("Unit must not be empty.");
        }

        if (errors.Count > 0)
        {
            return (null, errors);
        }

        var sensor = new SensorEntity(
            id,
            controllerId,
            type,
            unit.Trim(),
            lastValue,
            updatedAt,
            createdAt);

        return (sensor, errors);
    }

    public List<string>? Update(
        Guid controllerId,
        SensorTypeEnum type,
        string unit,
        double lastValue,
        DateTime updatedAt)
    {
        var errors = new List<string>();

        if (controllerId == Guid.Empty)
        {
            errors.Add("Controller id must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(unit))
        {
            errors.Add("Unit must not be empty.");
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        ControllerId = controllerId;
        Type = type;
        Unit = unit;
        LastValue = lastValue;
        UpdatedAt = updatedAt;

        return null;
    }
}
