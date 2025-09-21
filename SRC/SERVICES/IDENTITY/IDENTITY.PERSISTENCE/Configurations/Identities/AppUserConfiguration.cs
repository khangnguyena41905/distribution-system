using IDENTITY.DOMAIN.Entities.Identities;
using IDENTITY.PERSISTENCE.Contants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IDENTITY.PERSISTENCE.Configurations.Identities;

internal sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable(TableNames.AppUsers);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.IsDirector)
            .HasDefaultValue(false);

        builder.Property(x => x.IsHeadOfDepartment)
            .HasDefaultValue(false);

        builder.Property(x => x.ManagerId)
            .HasDefaultValue(null);

        builder.Property(x => x.IsReceipient)
            .HasDefaultValue(-1);

        builder.Property(x => x.AccountId)
            .IsRequired();

        // Each User can have many UserClaims
        builder.HasMany(e => e.Claims)
            .WithOne()
            .HasForeignKey(uc => uc.UserId)
            .IsRequired();

        // Each User can have many UserLogins
        builder.HasMany(e => e.Logins)
            .WithOne()
            .HasForeignKey(ul => ul.UserId)
            .IsRequired();

        // Each User can have many UserTokens
        builder.HasMany(e => e.Tokens)
            .WithOne()
            .HasForeignKey(ut => ut.UserId)
            .IsRequired();

        // Each User can have many UserTokens
        builder.HasMany(e => e.UserRoles)
            .WithOne()
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();
        
        builder.HasOne(u => u.Account)
            .WithOne()
            .HasForeignKey<AppUser>(u => u.AccountId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}