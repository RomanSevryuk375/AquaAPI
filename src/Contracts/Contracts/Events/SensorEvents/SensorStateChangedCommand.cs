using Contracts.Enums;

namespace Contracts.Events.SensorEvents;

public class SensorStateChangedCommand
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public Guid Id { get; init; }
    public SensorStateEnum State { get; init; }
}
