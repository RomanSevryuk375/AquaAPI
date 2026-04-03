namespace Contracts.Events;

public record SensorDeletedEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public Guid Id { get; init; }
}
