namespace Notification.Application.DTOs.MaintenanceLog;

public record MaintenanceLogResponseDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid AquariumId { get; init; }
    public DateTime ActionDate { get; init; }
    public double? PhLevel { get; init; }
    public double? KhLevel { get; init; }
    public double? No3Level { get; init; }
    public string Notes { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
