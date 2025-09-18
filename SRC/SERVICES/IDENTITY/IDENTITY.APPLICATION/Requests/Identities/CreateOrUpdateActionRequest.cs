namespace IDENTITY.APPLICATION.Requests.Identities;

public class CreateOrUpdateActionRequest
{
    public string Name { get; set; }
    public int? SortOrder { get; set; }
    public bool? IsActive { get; set; }
}

