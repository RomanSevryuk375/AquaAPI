namespace BuildingBlocks.IntegrationEvents.Events.Users;

public sealed record UserUpdatedEvent : BaseIntegrationEvent
{
    public Guid UserId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
}
