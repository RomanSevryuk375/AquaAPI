namespace BuildingBlocks.IntegrationEvents.Events.Relays;

public sealed record ChangeRelayStateEvent : BaseIntegrationEvent
{
    public Guid ControllerId { get; init; }
    public Guid RelayId { get; init; }
    public bool TargetState { get; init; }
    public DateTime? ExpireAt { get; init; }
}
