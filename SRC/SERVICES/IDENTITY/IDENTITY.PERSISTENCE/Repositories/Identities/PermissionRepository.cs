using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;
using IDENTITY.DOMAIN.Entities.Identities;
using Microsoft.EntityFrameworkCore;

namespace IDENTITY.PERSISTENCE.Repositories.Identities;


public class PermissionRepository : IPermissionRepository
{
    private readonly ApplicationDbContext _context;

    public PermissionRepository(ApplicationDbContext context) => _context = context;
    
    public async Task<IEnumerable<Permission>> GetByAppRoleIdAsync(Guid roleId)
    {
        var query = _context.Permissions;
        return await _context.Permissions.Where(x => x.RoleId == roleId).ToListAsync();
    }

    public async Task RemoveByAppRoleIdAsync(Guid roleId)
    {
        var needRemoveList = await _context.Permissions.Where(x => x.RoleId == roleId).ToListAsync();
        _context.RemoveRange(needRemoveList);
    }
}