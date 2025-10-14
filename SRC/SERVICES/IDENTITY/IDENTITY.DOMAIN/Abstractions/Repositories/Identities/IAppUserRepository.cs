using IDENTITY.DOMAIN.Entities.Identities;
using INVENTORY.DOMAIN.Abstractions.Repositories;

namespace IDENTITY.DOMAIN.Abstractions.Repositories.Identities;
public interface IAppUserRepository : IRepositoryBase<AppUser, Guid>
{
    Task AssignRoleAsync(AppUser user, AppRole role);
    Task<(AppUser? User, List<AppRole> Roles)> GetUserWithRolesAsync(Guid userId);
    Task<List<Function>> GetFunctionsWithActionsAsync(List<string> functionIds);
}
