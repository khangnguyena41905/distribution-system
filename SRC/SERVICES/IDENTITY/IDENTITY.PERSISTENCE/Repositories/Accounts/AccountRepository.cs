using IDENTITY.DOMAIN.Abstractions.Repositories.Accounts;
using IDENTITY.DOMAIN.Entities.Accounts;

namespace IDENTITY.PERSISTENCE.Repositories.Accounts;

internal class AccountRepository : RepositoryBase<Account, Guid>, IAccountRepository
{
    public AccountRepository(ApplicationDbContext context) : base(context)
    {
    }
}