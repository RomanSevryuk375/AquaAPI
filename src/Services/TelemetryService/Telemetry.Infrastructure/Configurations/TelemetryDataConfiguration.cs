using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Telemetry.Domain.Entities;

namespace Telemetry.Infrastructure.Configurations;

public class TelemetryDataConfiguration : IEntityTypeConfiguration<TelemetryRawEntity>
{
    void IEntityTypeConfiguration<TelemetryRawEntity>.Configure(EntityTypeBuilder<TelemetryRawEntity> builder)
    {
        builder.ToTable("telemetry_data");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SensorId).IsRequired();

        builder.Property(x => x.Value)
            .HasPrecision(10, 4)
            .IsRequired();

        builder.Property(x => x.ExternalMessageId)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.RecordedAt).IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasIndex(x => x.ExternalMessageId).IsUnique();

        builder.HasIndex(x => new 
        { 
            x.SensorId, 
            x.RecordedAt 
        });
    }
}
