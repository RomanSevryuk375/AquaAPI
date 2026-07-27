namespace BuildingBlocks.IntegrationEvents.Events.Relays;

public sealed record RelayDeletedEvent : BaseIntegrationEvent
{
    public Guid RelayId { get; init; }
}
