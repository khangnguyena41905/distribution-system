using IDENTITY.DOMAIN.Entities.Identities;

namespace IDENTITY.DOMAIN.Abstractions.Repositories.Identities;

public interface IAppUserRepository : IRepositoryBase<AppUser, Guid>;