using Contracts.Authorization;
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

        builder.Property(x => x.Permissions)
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(x => x.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasData(
            new
            {
                Id = Guid.Parse(SubscriptionEnum.Free),
                Name = "Free",
                Price = 0m,
                DurationDays = SubscriptionEnum.FreeDuration,
                Permissions = new List<string>
                {
                    SubPermissions.TankRead,
                    SubPermissions.TankCreate,
                    SubPermissions.TankUpdate,
                    SubPermissions.TankDelete,
                    SubPermissions.TankLimit1,
                    SubPermissions.DeviceControl,
                    SubPermissions.AutoRuleCreate,
                    SubPermissions.AutoRuleLimit5,
                    SubPermissions.AutoScheduleCreate,
                    SubPermissions.TelemetryView,
                    SubPermissions.MaintenanceLogRead,
                    SubPermissions.MaintenanceLogWrite,
                    SubPermissions.AccountUpdate,
                    SubPermissions.AccountView,
                },
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new
            {
                Id = Guid.Parse(SubscriptionEnum.Professional),
                Name = "Professional",
                Price = 9.99m,
                DurationDays = SubscriptionEnum.ProfessionalDuration,
                Permissions = new List<string>
                {
                    SubPermissions.TankRead,
                    SubPermissions.TankCreate,
                    SubPermissions.TankUpdate,
                    SubPermissions.TankDelete,
                    SubPermissions.TankLimit10,
                    SubPermissions.DeviceControl,
                    SubPermissions.AutoRuleCreate,
                    SubPermissions.AutoRuleLimit10,
                    SubPermissions.AutoScheduleCreate,
                    SubPermissions.TelemetryView,
                    SubPermissions.AnalyticsHistory,
                    SubPermissions.TelegramAlerts,
                    SubPermissions.MaintenanceLogRead,
                    SubPermissions.MaintenanceLogWrite,
                    SubPermissions.ReminderManage,
                    SubPermissions.AccountUpdate,
                    SubPermissions.AccountView,
                },
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            },
            new
            {
                Id = Guid.Parse(SubscriptionEnum.Elite),
                Name = "Elite",
                Price = 19.99m,
                DurationDays = SubscriptionEnum.EliteDuration,
                Permissions = new List<string>
                {
                    SubPermissions.TankRead,
                    SubPermissions.TankCreate,
                    SubPermissions.TankUpdate,
                    SubPermissions.TankDelete,
                    SubPermissions.TankLimitUnlimited,
                    SubPermissions.DeviceControl,
                    SubPermissions.DeviceEditManual,
                    SubPermissions.AutoRuleCreate,
                    SubPermissions.AutoRuleUnlimited,
                    SubPermissions.AutoScheduleCreate,
                    SubPermissions.VacationMode,
                    SubPermissions.TelemetryView,
                    SubPermissions.AnalyticsHistory,
                    SubPermissions.DiagnosticsFull,
                    SubPermissions.DataRealtime,
                    SubPermissions.MaintenanceLogRead,
                    SubPermissions.MaintenanceLogWrite,
                    SubPermissions.ReminderManage,
                    SubPermissions.EmailAlerts,
                    SubPermissions.TelegramAlerts,
                },
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }
        );
    }
}
