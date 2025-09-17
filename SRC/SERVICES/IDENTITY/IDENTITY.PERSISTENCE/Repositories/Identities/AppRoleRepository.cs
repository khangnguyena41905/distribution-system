using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;
using IDENTITY.DOMAIN.Entities.Identities;

namespace IDENTITY.PERSISTENCE.Repositories.Identities;

public class AppRoleRepository: RepositoryBase<AppRole, Guid>,IAppRoleRepository
{
    public AppRoleRepository(ApplicationDbContext context) : base(context)
    {
    }
}