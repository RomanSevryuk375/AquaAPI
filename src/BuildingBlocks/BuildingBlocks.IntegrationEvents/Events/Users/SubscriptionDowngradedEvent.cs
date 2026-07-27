namespace BuildingBlocks.IntegrationEvents.Events.Users;

public sealed record SubscriptionDowngradedEvent : BaseIntegrationEvent
{
    public Guid UserId { get; init; }
    public Guid NewSubscriptionId { get; init; }
}
