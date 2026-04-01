using Telemetry.Domain.Interfaces;

namespace Telemetry.Domain.Entities;

public sealed class TelemetryDataEntity : IEntity
{
    private TelemetryDataEntity(
        Guid id,
        Guid sensorId,
        double value,
        string externalMessageId,
        DateTime recordedAt,
        DateTime createdAt)
    {
        Id = id;
        SensorId = sensorId;
        Value = value;
        ExternalMessageId = externalMessageId;
        RecordedAt = recordedAt;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public Guid SensorId { get; private set; }
    public double Value { get; private set; }
    public string ExternalMessageId { get; private set; }
    public DateTime RecordedAt { get; private set; }
    public DateTime CreatedAt { get; private set; } 

    public static (TelemetryDataEntity? telemetryData, List<string> errors) Create(
        Guid id,
        Guid sensorId,
        double value,
        string externalMessageId,
        DateTime recordedAt,
        DateTime createdAt)
    {
        var errors = new List<string>();

        if (id == Guid.Empty)
        {
            errors.Add("Telemetry data id must not be empty.");
        }

        if (sensorId == Guid.Empty)
        {
            errors.Add("Sensor id must not be empty.");
        }

        if (recordedAt < createdAt)
        {
            errors.Add("RecordedAt must be greater than or equal to CreatedAt.");
        }

        if (errors.Count > 0)
        {
            return (null, errors);
        }

        id = Guid.NewGuid();

        createdAt = DateTime.UtcNow;

        var telemetryData = new TelemetryDataEntity(
            id,
            sensorId,
            value, 
            externalMessageId,
            recordedAt,
            createdAt);

        return (telemetryData, errors);
    }
}
