using IDENTITY.DOMAIN.Entities.Accounts;
using IDENTITY.DOMAIN.Entities.Identities;
using IDENTITY.PERSISTENCE.Contants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IDENTITY.PERSISTENCE.Configurations.Accounts;

public class ActionConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable(TableNames.Accounts);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserName)
            .HasMaxLength(200)
            .IsRequired(true);

        builder.Property(x => x.Password)
            .HasMaxLength(200)
            .IsRequired(true);
    }

}