using BuildingBlocks.Infrastructure.Data.Outbox;
using Microsoft.EntityFrameworkCore;
using Telemetry.Domain.Entities;

namespace Telemetry.Infrastructure.Persistence;

public class TelemetryDbContext(DbContextOptions<TelemetryDbContext> options) : DbContext(options)
{
    public DbSet<Ecosystem> Ecosystems => Set<Ecosystem>();
    public DbSet<Sensor> Sensors => Set<Sensor>();
    public DbSet<AggregateTelemetry> TelemetryAggregateData => Set<AggregateTelemetry>();
    public DbSet<RawTelemetry> TelemetryRawData => Set<RawTelemetry>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TelemetryDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
