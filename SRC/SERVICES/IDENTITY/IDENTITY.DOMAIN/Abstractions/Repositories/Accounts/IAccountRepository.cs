using IDENTITY.DOMAIN.Entities.Accounts;

namespace IDENTITY.DOMAIN.Abstractions.Repositories.Accounts;

public interface IAccountRepository: IRepositoryBase<Account, Guid>
{
}