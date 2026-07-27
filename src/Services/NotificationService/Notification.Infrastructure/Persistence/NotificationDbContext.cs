using Microsoft.EntityFrameworkCore;
using Notification.Domain.Entities;

namespace Notification.Infrastructure.Persistence;

public sealed class NotificationDbContext(DbContextOptions<NotificationDbContext> options) : DbContext(options)
{
    public DbSet<Ecosystem> Aquariums =>  Set<Ecosystem>();
    public DbSet<MaintenanceLog> MaintenanceLogs =>  Set<MaintenanceLog>();
    public DbSet<Domain.Entities.Notification> Notifications =>  Set<Domain.Entities.Notification>();
    public DbSet<Reminder> Reminders =>  Set<Reminder>();
    public DbSet<User> Users => Set<User>();
    public DbSet<BuildingBlocks.Infrastructure.Data.Outbox.OutboxMessage> OutboxMessages => Set<BuildingBlocks.Infrastructure.Data.Outbox.OutboxMessage>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotificationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
