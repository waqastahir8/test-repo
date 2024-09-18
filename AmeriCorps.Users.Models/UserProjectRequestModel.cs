namespace AmeriCorps.Users.Models;

public class UserProjectRequestModel
{

    public string ProjectName { get; set; } = string.Empty;

    public string ProjectCode { get; set; } = string.Empty;

    public string ProjectOrg { get; set; } = string.Empty;

    public string ProjectType { get; set; } = string.Empty;

    public bool Active { get; set; } = true;

    public List<ProjectRoleRequestModel> ProjectRoles { get; set; } = new List<ProjectRoleRequestModel>();

    public List<ProjectAccessRequestModel> ProjectAccess { get; set; } = new List<ProjectAccessRequestModel>();

}