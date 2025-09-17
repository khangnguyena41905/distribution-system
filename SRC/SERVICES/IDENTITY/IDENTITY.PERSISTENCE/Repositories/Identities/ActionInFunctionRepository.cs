using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;
using IDENTITY.DOMAIN.Entities.Identities;
using Microsoft.EntityFrameworkCore;

namespace IDENTITY.PERSISTENCE.Repositories.Identities;


public class ActionInFunctionRepository : IActionInFunctionRepository
{
    private readonly ApplicationDbContext _context;

    public ActionInFunctionRepository(ApplicationDbContext context)
        => _context = context;

    public async Task<IEnumerable<ActionInFunction>> FindByFunctionIdAsync(string functionId)
    {
        var query = _context.ActionInFunctions;
        return await query.Where(x => x.FunctionId == functionId).ToListAsync();
    }

    public Task RemoveListAsync(List<ActionInFunction> actionInFunctions)
    {
        _context.ActionInFunctions.RemoveRange(actionInFunctions);
        return Task.CompletedTask;
    }
    
    public async Task AddRangeAsync(List<ActionInFunction> list)
    {
        await _context.ActionInFunctions.AddRangeAsync(list);
    }
}