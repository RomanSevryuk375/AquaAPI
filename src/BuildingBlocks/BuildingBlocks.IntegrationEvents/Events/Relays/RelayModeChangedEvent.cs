namespace BuildingBlocks.IntegrationEvents.Events.Relays;

public sealed record RelayModeChangedEvent : BaseIntegrationEvent
{
    public Guid RelayId { get; init; }
    public bool IsManual { get; init; }
}
