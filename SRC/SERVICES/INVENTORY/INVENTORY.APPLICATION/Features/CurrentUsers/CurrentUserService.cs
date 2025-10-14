using COMMON.CONTRACT.Abstractions.Shared;
using Microsoft.AspNetCore.Http;

namespace INVENTORY.APPLICATION;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;
    public string? UserName => _httpContextAccessor.HttpContext?.User?.Identity?.Name;
}
