using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;
using IDENTITY.DOMAIN.Entities.Identities;

namespace IDENTITY.PERSISTENCE.Repositories.Identities;

public class FunctionRepository : RepositoryBase<Function, string>, IFunctionRepository
{
    public FunctionRepository(ApplicationDbContext context) : base(context)
    {
    }
}