namespace Contracts.Events.ControllerEvents;

public record ControllerNotOnlineEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public Guid ControllerId { get; init; }
    public DateTime LastSeenAt { get; init; }
}
