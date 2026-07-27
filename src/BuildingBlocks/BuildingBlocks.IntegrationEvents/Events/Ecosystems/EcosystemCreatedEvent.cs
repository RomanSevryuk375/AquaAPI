namespace BuildingBlocks.IntegrationEvents.Events.Ecosystems;

public sealed record EcosystemCreatedEvent : BaseIntegrationEvent
{
    public Guid EcosystemId { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid UserId { get; init; }
    public Guid ControllerId { get; init; }
}
