namespace Contracts.Events.EcosystemEvents;

public record EcosystemCreatedEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public Guid EcosystemId { get; init; }
    public Guid UserId { get; init; }
    public Guid ControllerId { get; init; }
}
