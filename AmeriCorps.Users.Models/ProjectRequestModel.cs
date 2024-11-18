namespace AmeriCorps.Users.Models;

public class ProjectRequestModel
{
    public int Id { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string ProjectOrgCode { get; set; } = string.Empty;
    public string ProjectCode { get; set; } = string.Empty;
    public int ProjectId { get; set; } //UEI Number
    public int GspProjectId { get; set; } //GSP Auto Assigned ID

    public string ProgramName { get; set; } = string.Empty;
    public string ProgramYear { get; set; } = string.Empty;

    public string StreetAddress { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;

    public DateOnly? ProjectPeriodStartDt { get; set; }
    public DateOnly? ProjectPeriodEndDt { get; set; }

    public DateOnly? EnrollmentStartDt { get; set; }
    public DateOnly? EnrollmentEndDt { get; set; }

    public List<OperatingSiteRequestModel> OperatingSites { get; set; } = new List<OperatingSiteRequestModel>();
    public List<SubGranteeRequestModel> SubGrantees { get; set; } = new List<SubGranteeRequestModel>();

    public string ProjectType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool Active { get; set; } = true;

    public double TotalAwardedMsys { get; set; }

    public double LivingAllowanceMsys { get; set; }

    public double NonLivingAllowanceMsys { get; set; }
}