namespace BuildingBlocks.IntegrationEvents.Events.Sensors;

public sealed record SensorNoDataAlertEvent : BaseIntegrationEvent
{
    public Guid UserId { get; init; }
    public Guid EcosystemId { get; init; }
    public Guid SensorId { get; init; }
    public DateTime LastSeenAt { get; init; }
}
