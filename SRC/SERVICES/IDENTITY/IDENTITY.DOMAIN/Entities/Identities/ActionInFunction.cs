namespace IDENTITY.DOMAIN.Entities.Identities;

public class ActionInFunction
{
    public string ActionId { get; set; }
    public string FunctionId { get; set; }
    public virtual ICollection<Permission> Permissions { get; set; }
}