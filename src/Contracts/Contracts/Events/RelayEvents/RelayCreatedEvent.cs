using Contracts.Enums;

namespace Contracts.Events.RelayEvents;

public record RelayCreatedEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public Guid RelayId { get; init; }
    public RelayPurposeEnum Purpose { get; init; }
    public bool IsManual { get; init; }
}
