using IDENTITY.DOMAIN.Entities.Accounts;
using INVENTORY.DOMAIN.Abstractions.Repositories;

namespace IDENTITY.DOMAIN.Abstractions.Repositories.Accounts;

public interface IAccountRepository: IRepositoryBase<Account, Guid>
{
}