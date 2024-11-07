namespace AmeriCorps.Users.Data.Core;

public sealed class UserProject : EntityWithUserId
{
    public string ProjectName { get; set; } = string.Empty;

    public string ProjectCode { get; set; } = string.Empty;

    public string ProjectType { get; set; } = string.Empty;

    public string ProjectOrg { get; set; } = string.Empty;

    public bool Active { get; set; } = true;

    public List<ProjectRole> ProjectRoles { get; set; } = [];

    public List<ProjectAccess> ProjectAccess { get; set; } = [];
}