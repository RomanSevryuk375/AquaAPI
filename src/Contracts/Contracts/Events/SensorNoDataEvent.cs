namespace Contracts.Events;

public record SensorNoDataEvent 
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public Guid SensorId { get; init; } 
    public DateTime LastSeenAt { get; init; }
}
