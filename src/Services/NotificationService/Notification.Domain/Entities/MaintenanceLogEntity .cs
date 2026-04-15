using Notification.Domain.Interfaces;

namespace Notification.Domain.Entities;

public class MaintenanceLogEntity : IEntity
{
    private MaintenanceLogEntity(
        Guid id, 
        Guid userId, 
        Guid aquariumId, 
        DateTime actionDate, 
        double? ph, 
        double? kh, 
        double? no3, 
        string notes, 
        DateTime createdAt)
    {
        Id = id;
        UserId = userId;
        AquariumId = aquariumId;
        ActionDate = actionDate;
        PhLevel = ph;
        KhLevel = kh;
        No3Level = no3;
        Notes = notes;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid AquariumId { get; private set; }
    public DateTime ActionDate { get; private set; } 
    public double? PhLevel { get; private set; }
    public double? KhLevel { get; private set; }
    public double? No3Level { get; private set; }
    public string Notes { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    public static (MaintenanceLogEntity? log, List<string> errors) Create(
        Guid userId, 
        Guid aquariumId, 
        DateTime actionDate, 
        double? ph, 
        double? kh, 
        double? no3, 
        string notes)
    {
        var errors = new List<string>();
        if (ph is < 0 or > 14)
        {
            errors.Add("pH must be between 0 and 14");
        }

        if (errors.Count > 0)
        {
            return (null, errors);
        }

        if (actionDate > DateTime.UtcNow.AddMinutes(5))
        {
            errors.Add("Action date cannot be in the future.");
        }

        var log = new MaintenanceLogEntity(
            Guid.NewGuid(), 
            userId, 
            aquariumId, 
            actionDate, 
            ph, 
            kh, 
            no3, 
            notes.Trim(), 
            DateTime.UtcNow);

        return (log, errors);
    }
}