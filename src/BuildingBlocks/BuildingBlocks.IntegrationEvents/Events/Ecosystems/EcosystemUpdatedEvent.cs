namespace BuildingBlocks.IntegrationEvents.Events.Ecosystems;

public sealed record EcosystemUpdatedEvent : BaseIntegrationEvent
{
    public Guid EcosystemId { get; init; }
    public Guid UserId { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid ControllerId { get; init; }
    public DateTime CreatedAt { get; init; }
}
