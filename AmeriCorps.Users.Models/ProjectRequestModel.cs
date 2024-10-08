namespace AmeriCorps.Users.Models;

public class ProjectRequestModel
{

    public string ProjectName { get; set; } = string.Empty;

    public string ProjectCode { get; set; } = string.Empty;

    public string ProjectOrg { get; set; } = string.Empty;

    public string ProjectType { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public List<OperatingSiteRequestModel> OperatingSites { get; set; } = new List<OperatingSiteRequestModel>();

}