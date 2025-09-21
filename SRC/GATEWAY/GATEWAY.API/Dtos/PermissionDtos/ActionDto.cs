namespace GATEWAY.API.Dtos.PermissionDtos;

public class ActionDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int? SortOrder { get; set; }
    public bool? IsActive { get; set; }
}
