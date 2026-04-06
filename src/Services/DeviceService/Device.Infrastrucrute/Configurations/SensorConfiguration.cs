using Device.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Device.Infrastructure.Configurations;

public class SensorConfiguration : IEntityTypeConfiguration<SensorEntity>
{
    public void Configure(EntityTypeBuilder<SensorEntity> builder)
    {
        builder.ToTable("sensors");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ControllerId).IsRequired();
        builder.Property(x => x.HardwarePin)
            .HasMaxLength(32)
            .IsRequired();

        builder.HasIndex(x => new 
        { 
            x.ControllerId, 
            x.HardwarePin 
        }).IsUnique();

        builder.Property(x => x.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Unit)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired();
    }
}
