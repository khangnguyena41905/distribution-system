namespace IDENTITY.APPLICATION.Dtos.PermissionDtos;

public class ActionInFunctionDto
{
    public string ActionId { get; set; }
    public string FunctionId { get; set; }
    public List<PermissionDto> Permissions { get; set; }
}