using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;
using Action = IDENTITY.DOMAIN.Entities.Identities.Action;

namespace IDENTITY.PERSISTENCE.Repositories.Identities;

public class ActionRepository : RepositoryBase<Action, string>,IActionRepository
{
    public ActionRepository(ApplicationDbContext context) : base(context)
    {
    }
}