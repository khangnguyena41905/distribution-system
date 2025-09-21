using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;
using IDENTITY.DOMAIN.Entities.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IDENTITY.PERSISTENCE.Repositories.Identities;

public class AppUserRepository : RepositoryBase<AppUser, Guid>, IAppUserRepository
{
    public AppUserRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task AssignRoleAsync(AppUser user, AppRole role)
    {
        bool exists = await _context.UserRoles
            .AnyAsync(ur => ur.UserId == user.Id && ur.RoleId == role.Id);

        if (!exists)
        {
            var userRole = new IdentityUserRole<Guid>
            {
                UserId = user.Id,
                RoleId = role.Id
            };

            _context.UserRoles.Add(userRole);

            user.UserRoles ??= new List<IdentityUserRole<Guid>>();
            role.UserRoles ??= new List<IdentityUserRole<Guid>>();

            user.UserRoles.Add(userRole);
            role.UserRoles.Add(userRole);

            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<(AppUser? User, List<AppRole> Roles)> GetUserWithRolesAsync(Guid userId)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return (null, new List<AppRole>());

        var roleIds = user.UserRoles.Select(ur => ur.RoleId).ToList();

        var roles = await _context.Set<AppRole>()
            .Where(r => roleIds.Contains(r.Id))
            .ToListAsync();

        return (user, roles);
    }

    public async Task<List<Function>> GetFunctionsWithActionsAsync(List<string> functionIds)
    {
        return await _context.Functions
            .Where(f => functionIds.Contains(f.Id))
            .Include(f => f.ActionInFunctions)
            .ToListAsync();
    }
    
    
}