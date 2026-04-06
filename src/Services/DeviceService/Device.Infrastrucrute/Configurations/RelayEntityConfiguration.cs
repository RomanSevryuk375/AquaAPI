using Device.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Device.Infrastructure.Configurations;

public class RelayEntityConfiguration : IEntityTypeConfiguration<RelayEntity>
{
    public void Configure(EntityTypeBuilder<RelayEntity> builder)
    {
        builder.ToTable("relays");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ControllerId).IsRequired();

        builder.Property(x => x.HardwarePin)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.Purpose)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.IsManual).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasIndex(x => new 
        { 
            x.ControllerId, 
            x.HardwarePin 
        }).IsUnique();
    }
}
