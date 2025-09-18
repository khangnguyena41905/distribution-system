using IDENTITY.DOMAIN.Entities.Identities;

namespace IDENTITY.APPLICATION.Requests.Identities;

public class CreateOrUpdateFunctionRequest
{
    public string Name { get; set; }
    public string Url { get; set; }
    public string? ParrentId { get; set; }
    public int? SortOrder { get; set; }
    public string? CssClass { get; set; }
    public bool? IsActive { get; set; }
    public ICollection<ActionInFunction>? ActionInFunctions { get; set; }
}