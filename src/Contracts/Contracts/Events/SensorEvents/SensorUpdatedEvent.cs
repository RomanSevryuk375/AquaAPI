using Contracts.Enums;

namespace Contracts.Events.SensorEvents;

public class SensorUpdatedEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public Guid Id { get; init; }
    public Guid ControllerId { get; init; }
    public SensorTypeEnum Type { get; init; }
    public string Unit { get; init; } = string.Empty;
    public double LastValue { get; init; }
    public DateTime UpdatedAt { get; init; }
    public DateTime CreatedAt { get; init; }
}
