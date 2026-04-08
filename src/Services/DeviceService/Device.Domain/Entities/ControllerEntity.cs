using Device.Domain.Interfaces;

namespace Device.Domain.Entities;

public sealed class ControllerEntity : IEntity
{
    private ControllerEntity(
        Guid id, 
        string macAddress, 
        string name, 
        bool isOnline, 
        DateTime lastSeenAt, 
        DateTime createdAt)
    {
        Id = id;
        MacAddress = macAddress;
        Name = name;
        IsOnline = isOnline;
        LastSeenAt = lastSeenAt;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public string MacAddress { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public bool IsOnline { get; private set; }
    public DateTime LastSeenAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static (ControllerEntity? controller, List<string>? errors) Create(
        string macAddress,
        string name,
        bool isOnline)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(macAddress))
        {
            errors.Add("macAdress must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            errors.Add("name must not be empty.");
        }

        if (errors.Count > 0)
        {
            return (null, errors);
        }

        var controller = new ControllerEntity(
            Guid.NewGuid(),
            macAddress.Trim(),
            name.Trim(),
            isOnline,
            DateTime.UtcNow,
            DateTime.UtcNow);

        return (controller, errors);
    }

    public List<string>? Update(
        string macAddress,
        string name)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(macAddress))
        {
            errors.Add("macAdress must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            errors.Add("name must not be empty.");
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        MacAddress = macAddress.Trim();
        Name = name.Trim();
        
        return null;
    }

    public void RecordPing()
    {
        LastSeenAt = DateTime.UtcNow;
    }

    public void ToggleState()
    {
        IsOnline = !IsOnline;
    }

    public void SetOffline()
    {
        IsOnline = false;
    }
}
