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
        Guid sensorId,
        double value,
        string externalMessageId,
        DateTime recordedAt)
    {
        var errors = new List<string>();

        if (sensorId == Guid.Empty)
        {
            errors.Add("sensorId must not be empty.");
        }

        if (recordedAt > DateTime.UtcNow.AddMinutes(1))
        {
            errors.Add("recordedAt cannot be in the future.");
        }

        if (string.IsNullOrWhiteSpace(externalMessageId))
        {
            errors.Add("externalMessageId must not be empty.");
        }

        if (errors.Count > 0)
        {
            return (null, errors);
        }

        var telemetryData = new TelemetryDataEntity(
            Guid.NewGuid(),
            sensorId,
            value, 
            externalMessageId,
            recordedAt,
            DateTime.UtcNow);

        return (telemetryData, errors);
    }
}
