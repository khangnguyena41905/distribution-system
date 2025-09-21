using IDENTITY.DOMAIN.Entities.Identities;
using IDENTITY.PERSISTENCE.Contants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IDENTITY.PERSISTENCE.Configurations.Identities;

public class FunctionConfiguration: IEntityTypeConfiguration<Function>
{
    public void Configure(EntityTypeBuilder<Function> builder)
    {
        builder.ToTable(TableNames.Functions); // tên bảng

        builder.HasKey(x => x.Id); // Khóa chính

        builder.Property(x => x.Id)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Url)
            .HasMaxLength(200);

        builder.Property(x => x.ParrentId)
            .HasMaxLength(50);

        builder.Property(x => x.CssClass)
            .HasMaxLength(50);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.Property(x => x.SortOrder)
            .HasDefaultValue(null);

        // // Mỗi Function có nhiều Permission
        // builder.HasMany(x => x.Permissions)
        //     .WithOne()
        //     .HasForeignKey(p => p.FunctionId)
        //     .IsRequired()
        //     .OnDelete(DeleteBehavior.Cascade);

        // Mỗi Function có nhiều ActionInFunction
        builder.HasMany(x => x.ActionInFunctions)
            .WithOne()
            .HasForeignKey(a => a.FunctionId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}