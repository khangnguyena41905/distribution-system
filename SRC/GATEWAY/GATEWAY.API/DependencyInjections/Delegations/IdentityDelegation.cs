using System.Net;
using GATEWAY.API.Services;

namespace GATEWAY.API.DependencyInjections.Delegations;

public class IdentityDelegation : DelegatingHandler
{
    private readonly IPermissionCacheService _permissionCacheService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityDelegation(IPermissionCacheService permissionCacheService, IHttpContextAccessor httpContextAccessor)
    {
        _permissionCacheService = permissionCacheService;
        _httpContextAccessor = httpContextAccessor;
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken token)
    {
        request.Headers.Add("InventoryHeader", "X-Inventory");

        var httpContext = _httpContextAccessor.HttpContext;
        var user = httpContext?.User;

        if (user?.Identity?.IsAuthenticated == true)
        {

            var userIdClaim = user.FindFirst("sub")?.Value;
            if (Guid.TryParse(userIdClaim, out Guid userId))
            {
                var permissions = await _permissionCacheService.GetUserPermissionsAsync(userId);
                if(permissions == null)
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                
                var accepted = permissions.Where(x => AcceptedFunctions.Contains(x.Name));
                if(!accepted.Any())
                    return new HttpResponseMessage(HttpStatusCode.Forbidden)
                    {
                        Content = new StringContent("Permission denied")
                    };

                var validationList = accepted
                    .SelectMany(x => x.ActionInFunctions
                        .Select(af => $"{x.Name.ToUpper()}.{af.ActionId.ToUpper()}"))
                    .ToList(); 
                request.Headers.Add("X-Validation", validationList);

            }

        }

        var response = await base.SendAsync(request, token);
        return response;
    }
    
    private List<string> AcceptedFunctions { get; set; } = new List<string> { "Identity" };

}