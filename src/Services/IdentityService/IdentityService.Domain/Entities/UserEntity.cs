using IdentityService.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Domain.Entities;

public class UserEntity : IdentityUser<Guid>, IEntity
{
    private UserEntity() { }

    private UserEntity(string name, string email, Guid subscriptionId)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        UserName = email; 
        SubscriptionId = subscriptionId;
        SubscriptionEndDate = DateTime.UtcNow; 
        CreatedAt = DateTime.UtcNow;
    }

    public string Name { get; private set; } = string.Empty;
    public Guid SubscriptionId { get; private set; }
    public DateTime SubscriptionEndDate { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static (UserEntity? user, List<string> errors) Create(string name, string email, Guid subscriptionId)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(name))
        {
            errors.Add("Name is required");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            errors.Add("Email is required");
        }

        if (errors.Count > 0)
        {
            return (null, errors);
        }

        var user = new UserEntity(
            name.Trim(),
            email.Trim(),
            subscriptionId);

        return (user, errors);
    }

    public void SetSubscription(Guid subscriptionId, int durationDays)
    {
        SubscriptionId = subscriptionId;
        SubscriptionEndDate = DateTime.UtcNow.AddDays(durationDays);
    }
}