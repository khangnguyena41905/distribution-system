using IDENTITY.DOMAIN.Entities.Identities;
using IDENTITY.PERSISTENCE.Contants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IDENTITY.PERSISTENCE.Configurations.Identities;

public class PermissionConfiguration: IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(TableNames.Permissions);

        // Composite key: RoleId + FunctionId + ActionId
        builder.HasKey(x => new { x.RoleId, x.FunctionId, x.ActionId });

        // Optional: Add constraints if needed
        builder.Property(x => x.FunctionId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.ActionId)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasOne(p => p.ActionInFunction)
            .WithMany(aif => aif.Permissions)
            .HasForeignKey(p => new { p.ActionId, p.FunctionId })
            .HasPrincipalKey(aif => new { aif.ActionId, aif.FunctionId });

        builder.HasOne(p => p.Role)
            .WithMany(r => r.Permissions)
            .HasForeignKey(p => p.RoleId);
    }
}