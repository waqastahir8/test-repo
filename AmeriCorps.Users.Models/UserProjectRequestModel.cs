namespace AmeriCorps.Users.Models;

public class UserProjectRequestModel
{

    public string ProjectName { get; set; } = string.Empty;

    public string ProjectCode { get; set; } = string.Empty;

    public string ProjectOrg { get; set; } = string.Empty;

    public string ProjectType { get; set; } = string.Empty;

    public bool Active { get; set; } = true;

}