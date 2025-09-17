using IDENTITY.DOMAIN.Entities.Identities;

namespace IDENTITY.DOMAIN.Abstractions.Repositories.Identities;

public interface IPermissionRepository
{
    Task<IEnumerable<Permission>> GetByAppRoleIdAsync(Guid roleId);
    Task RemoveByAppRoleIdAsync(Guid roleId);
}