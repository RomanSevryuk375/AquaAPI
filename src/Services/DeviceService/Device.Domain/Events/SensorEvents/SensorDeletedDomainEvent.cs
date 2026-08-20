using BuildingBlocks.Domain.Abstractions;

namespace Device.Domain.Events.SensorEvents;

public sealed record SensorDeletedDomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public Guid SensorId { get; init; }
    public Guid UserId { get; init; }
}
