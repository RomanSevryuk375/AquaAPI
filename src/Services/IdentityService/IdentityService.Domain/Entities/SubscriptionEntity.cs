using IdentityService.Domain.Interfaces;

namespace IdentityService.Domain.Entities;

public class SubscriptionEntity : IEntity
{
    private SubscriptionEntity() { }
    private SubscriptionEntity(
        Guid id, 
        string name, 
        decimal price, 
        int durationDays, 
        int maxAquariums, 
        bool canUseAnalytics)
    {
        Id = id;
        Name = name;
        Price = price;
        DurationDays = durationDays;
        MaxAquariums = maxAquariums;
        CanUseAnalytics = canUseAnalytics;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public int DurationDays { get; private set; }
    public int MaxAquariums { get; private set; }
    public bool CanUseAnalytics { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static SubscriptionEntity Create(
        Guid id, 
        string name, 
        decimal price, 
        int durationDays, 
        int maxAquariums, 
        bool canUseAnalytics)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name required");
        }

        return new SubscriptionEntity(
            id, 
            name, 
            price, 
            durationDays, 
            maxAquariums, 
            canUseAnalytics);
    }
}
