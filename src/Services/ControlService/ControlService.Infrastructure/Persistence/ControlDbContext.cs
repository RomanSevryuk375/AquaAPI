using BuildingBlocks.Infrastructure.Data.Outbox;
using Control.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Control.Infrastructure.Persistence;

public class ControlDbContext(DbContextOptions<ControlDbContext> options) : DbContext(options)
{
    public DbSet<AutomationRule> Rules => Set<AutomationRule>();
    public DbSet<Ecosystem> Ecosystems => Set<Ecosystem>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<Relay> Relays => Set<Relay>();
    public DbSet<RuleCondition> RuleConditions => Set<RuleCondition>();
    public DbSet<Schedule> Schedules => Set<Schedule>();
    public DbSet<Sensor> Sensors => Set<Sensor>();
    public DbSet<VacationMode> Vacations => Set<VacationMode>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ControlDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
