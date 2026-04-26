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

        builder.Property(x => x.Name)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.ConnectionProtocol)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.ConnectionAddress)
           .HasMaxLength(32)
           .IsRequired();

        builder.Property(x => x.IsNormalyOpen).IsRequired();

        builder.Property(x => x.Purpose)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.IsManual).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();

        builder.HasIndex(x => new 
        { 
            x.ControllerId, 
            x.ConnectionAddress
        }).IsUnique();

        builder.HasMany<RelayCommandsQueueEntity>()
            .WithOne()
            .HasForeignKey(x => x.RelayId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
