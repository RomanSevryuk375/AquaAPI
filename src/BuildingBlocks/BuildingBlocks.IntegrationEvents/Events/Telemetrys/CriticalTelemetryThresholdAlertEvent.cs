namespace BuildingBlocks.IntegrationEvents.Events.Telemetrys;

public sealed record CriticalTelemetryThresholdAlertEvent : BaseIntegrationEvent
{
    public Guid UserId { get; init; }
    public Guid EcosystemId { get; init; }
    public Guid SensorId { get; init; }
    public double Value { get; init; }
    public DateTime RecordedAt { get; init; }
    public Guid RelayId { get; init; }
    public bool RelayState { get; init; }
}
