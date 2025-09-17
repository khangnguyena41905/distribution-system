using IDENTITY.DOMAIN.Entities.Identities;

namespace IDENTITY.DOMAIN.Abstractions.Repositories.Identities;

public interface IActionInFunctionRepository
{
    Task<IEnumerable<ActionInFunction>> FindByFunctionIdAsync(string functionId);
    
    Task RemoveListAsync(List<ActionInFunction> actionInFunctions);
    Task AddRangeAsync(List<ActionInFunction> list);
}