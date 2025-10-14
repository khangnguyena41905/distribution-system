using INVENTORY.DOMAIN.Abstractions.Repositories;

namespace IDENTITY.DOMAIN.Abstractions.Repositories.Identities;
using Action = IDENTITY.DOMAIN.Entities.Identities.Action;

public interface IActionRepository : IRepositoryBase<Action, string>
{
    
}