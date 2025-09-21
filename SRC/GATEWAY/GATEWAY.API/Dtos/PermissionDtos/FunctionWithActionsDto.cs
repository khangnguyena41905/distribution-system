namespace GATEWAY.API.Dtos.PermissionDtos;

public class FunctionWithActionsDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public string? ParrentId { get; set; }
    public int? SortOrder { get; set; }
    public string? CssClass { get; set; }
    public bool? IsActive { get; set; }

    public List<ActionDto> Actions { get; set; } = new();
}

