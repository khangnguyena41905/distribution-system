namespace GATEWAY.API.Dtos.PermissionDtos;

public class UserPermissionDto
{
    public string FunctionId { get; set; }
    public List<string> ActionIds { get; set; }
}
