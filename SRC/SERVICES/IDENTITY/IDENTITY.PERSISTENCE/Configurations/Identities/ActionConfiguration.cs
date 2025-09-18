using IDENTITY.PERSISTENCE.Contants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Action = IDENTITY.DOMAIN.Entities.Identities.Action;

namespace IDENTITY.PERSISTENCE.Configurations.Identities;

public class ActionConfiguration : IEntityTypeConfiguration<Action>
{
    public void Configure(EntityTypeBuilder<Action> builder)
    {
        builder.ToTable(TableNames.Actions);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasMaxLength(50);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired(true);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.Property(x => x.SortOrder)
            .HasDefaultValue(null);

        // Each Action can have many Permissions
        builder.HasMany(e => e.Permissions)
            .WithOne()
            .HasForeignKey(p => p.ActionId)
            .IsRequired();

        // Each Action can have many ActionInFunctions
        builder.HasMany(e => e.ActionInFunctions)
            .WithOne()
            .HasForeignKey(aif => aif.ActionId)
            .IsRequired();
    }

}