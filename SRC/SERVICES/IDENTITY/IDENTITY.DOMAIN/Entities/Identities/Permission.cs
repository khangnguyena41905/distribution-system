using System.ComponentModel.DataAnnotations;

namespace IDENTITY.DOMAIN.Entities.Identities;

public class Permission
{
    public Guid RoleId { get; set; }
    [MaxLength(50)]
    public string FunctionId { get; set; }
    [MaxLength(50)]
    public string ActionId { get; set; }
    
    public virtual AppRole Role { get; set; }
    public virtual ActionInFunction ActionInFunction { get; set; }
}