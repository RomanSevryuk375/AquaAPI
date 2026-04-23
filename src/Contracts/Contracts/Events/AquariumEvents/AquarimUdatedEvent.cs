namespace Contracts.Events.AquariumEvents;

public class AquarimUdatedEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public Guid AquriumId { get; init; }
    public Guid UserId { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid ControllerId { get; init; }
    public DateTime CreatedAt { get; init; }
}
