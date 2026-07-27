namespace BuildingBlocks.IntegrationEvents.Events.Ecosystems;

public sealed record EcosystemDeletedEvent : BaseIntegrationEvent
{
    public Guid EcosystemId { get; init; }
}
