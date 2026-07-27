using BuildingBlocks.Infrastructure.Data.Outbox;

namespace Device.Infrastructure.Persistence;

public sealed class DeviceDbContext(DbContextOptions<DeviceDbContext> options)
    : DbContext(options)
{
    public DbSet<Controller> Controllers => Set<Controller>();
    public DbSet<RelayCommand> RelayCommands => Set<RelayCommand>();
    public DbSet<Relay> Relays => Set<Relay>();
    public DbSet<Sensor> Sensors => Set<Sensor>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DeviceDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
