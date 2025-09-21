namespace IDENTITY.APPLICATION.Dtos.PermissionDtos;

public class FunctionDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public bool? IsActive { get; set; }
    public List<ActionInFunctionDto> ActionInFunctions { get; set; }
}