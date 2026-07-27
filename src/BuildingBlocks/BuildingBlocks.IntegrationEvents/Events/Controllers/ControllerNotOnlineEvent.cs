namespace BuildingBlocks.IntegrationEvents.Events.Controllers;

public sealed record ControllerNotOnlineEvent : BaseIntegrationEvent
{
    public Guid UserId { get; init; }
    public Guid ControllerId { get; init; }
    public DateTime LastSeenAt { get; init; }
}
