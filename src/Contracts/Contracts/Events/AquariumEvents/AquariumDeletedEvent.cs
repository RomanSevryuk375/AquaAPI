namespace Contracts.Events.AquariumEvents;

public record AquariumDeletedEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public Guid AquriumId { get; init; }
}
