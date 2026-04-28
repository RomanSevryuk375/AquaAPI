using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Telemetry.Domain.Entities;

namespace Telemetry.Infrastructure.Configurations;

public class SensorConfiguration : IEntityTypeConfiguration<SensorEntity>
{
    void IEntityTypeConfiguration<SensorEntity>.Configure(EntityTypeBuilder<SensorEntity> builder)
    {
        builder.ToTable("sensors");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ControllerId).IsRequired();

        builder.Property(x => x.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.State)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Unit)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.LastValue)
            .HasPrecision(10, 4)
            .IsRequired();

        builder.Property(x => x.UpdatedAt).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.IsDataDelayed).IsRequired();

        builder.HasMany<TelemetryRawEntity>()
            .WithOne()
            .HasForeignKey(x => x.SensorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
