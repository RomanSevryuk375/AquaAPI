using Contracts.Enums;
using IdentityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Infrastructure.Configurations;

public class SubscriptionEntityConfigurations 
    : IEntityTypeConfiguration<SubscriptionEntity>
{
    public void Configure(EntityTypeBuilder<SubscriptionEntity> builder)
    {
        builder.ToTable("subscriptions");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Price).HasPrecision(18, 2);

        builder.HasData(
            new
            {
                Id = Guid.Parse(SubscriptionEnum.Free),
                Name = nameof(SubscriptionEnum.Free),
                Price = 0m,
                DurationDays = 36500, 
                MaxAquariums = 1,
                CanUseAnalytics = false,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new
            {
                Id = Guid.Parse(SubscriptionEnum.Professional),
                Name = nameof(SubscriptionEnum.Professional),
                Price = 9.99m,
                DurationDays = 30,
                MaxAquariums = 10,
                CanUseAnalytics = true,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new
            {
                Id = Guid.Parse(SubscriptionEnum.Elite),
                Name = nameof(SubscriptionEnum.Elite),
                Price = 19.99m,
                DurationDays = 30,
                MaxAquariums = 30,
                CanUseAnalytics = true,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
