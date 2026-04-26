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
        builder.Property(x => x.Name)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.ConnectionProtocol)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.ConnectionAddress)
           .HasMaxLength(32)
           .IsRequired();

        builder.Property(x => x.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.State)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Unit)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasIndex(x => new
        {
            x.ControllerId,
            x.ConnectionAddress
        }).IsUnique();
    }
}
