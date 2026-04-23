namespace Notification.Domain.SpecificationParams;

public record ReminderSpecificationParams
{
    public Guid? UserId { get; init; }
    public Guid? AquariumId { get; init; }
    public string? SearchTerm { get; init; } = string.Empty;

    public DateTime? LastDoneAtFrom { get; init; }
    public DateTime? LastDoneAtTo { get; init; }
    public DateTime? NextDueAtFrom { get; init; }
    public DateTime? NextDueAtTo { get; init; }
}
