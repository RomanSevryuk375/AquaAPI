using BuildingBlocks.Domain.Abstractions;

namespace Device.Domain.Events.RelayEvents;

public sealed record RelayDeletedDomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public Guid UserId { get; init; }
    public Guid RelayId { get; init; }
}
