using IDENTITY.DOMAIN.Entities.Identities;
using INVENTORY.DOMAIN.Abstractions.Repositories;

namespace IDENTITY.DOMAIN.Abstractions.Repositories.Identities;

public interface IAppRoleRepository: IRepositoryBase<AppRole, Guid>
{
    
}