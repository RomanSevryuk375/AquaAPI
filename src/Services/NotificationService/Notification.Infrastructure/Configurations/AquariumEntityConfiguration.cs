using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notification.Domain.Entities;

namespace Notification.Infrastructure.Configurations;

public class AquariumEntityConfiguration
    : IEntityTypeConfiguration<AquariumEntity>
{
    public void Configure(EntityTypeBuilder<AquariumEntity> builder)
    {
        builder.ToTable("aquariums");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).IsRequired();
        builder.HasIndex(x => x.UserId);

        builder.Property(x => x.Name)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired();
    }
}
