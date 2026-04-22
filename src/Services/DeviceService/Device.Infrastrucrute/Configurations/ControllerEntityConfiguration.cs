using Device.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Device.Infrastructure.Configurations;

public class ControllerEntityConfiguration : IEntityTypeConfiguration<ControllerEntity>
{
    public void Configure(EntityTypeBuilder<ControllerEntity> builder)
    {
        builder.ToTable("controllers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).IsRequired();
        builder.HasIndex(x => x.UserId);

        builder.Property(x => x.MacAddress)
            .HasMaxLength(17)
            .IsRequired();
        builder.HasIndex(x => x.MacAddress).IsUnique();

        builder.Property(x => x.Name)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.IsOnline).IsRequired();
        builder.Property(x => x.LastSeenAt).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasMany<SensorEntity>()
            .WithOne()
            .HasForeignKey(s => s.ControllerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany<RelayEntity>()
            .WithOne()
            .HasForeignKey(r => r.ControllerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
