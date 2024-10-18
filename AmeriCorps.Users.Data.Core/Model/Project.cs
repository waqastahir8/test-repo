using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AmeriCorps.Users.Data.Core;

[Index(nameof(ProjectCode))]
public sealed class Project : Entity
{
    [Column(TypeName = "varchar(64)")]
    public string ProjectName { get; set; } = string.Empty;

    [Column(TypeName = "varchar(8)")]
    public string ProjectOrgCode { get; set; } = string.Empty;

    [Column(TypeName = "varchar(8)")]
    public string ProjectCode { get; set; } = string.Empty;

    [Column(TypeName = "bigint")]
    public int ProjectId { get; set; } //UEI Number

    [Column(TypeName = "bigint")]
    public int GspProjectId { get; set; } //GSP Auto Assigned ID

    [Column(TypeName = "varchar(64)")]
    public string ProgramName { get; set; } = string.Empty;

    public string ProgramYear { get; set; } = string.Empty;

    public Award? Award { get; set; }

    public User? AuthorizedRep { get; set; }
    public User? ProjectDirector { get; set; }

    [Column(TypeName = "varchar(64)")]
    public string StreetAddress { get; set; } = string.Empty;

    [Column(TypeName = "varchar(16)")]
    public string City { get; set; } = string.Empty;

    [Column(TypeName = "varchar(8)")]
    public string State { get; set; } = string.Empty;

    [Column(TypeName = "varchar(8)")]
    public string ZipCode { get; set; } = string.Empty;

    public DateOnly? ProjectPeriodStartDt { get; set; }
    public DateOnly? ProjectPeriodEndDt { get; set; }

    public DateOnly? EnrollmentStartDt { get; set; }
    public DateOnly? EnrollmentEndDt { get; set; }

    public List<OperatingSite> OperatingSites { get; set; } = new List<OperatingSite>();
    public List<SubGrantee> SubGrantees { get; set; } = new List<SubGrantee>();

    [Column(TypeName = "varchar(16)")]
    public string ProjectType { get; set; } = string.Empty;

    [Column(TypeName = "varchar(64)")]
    public string Description { get; set; } = string.Empty;

    public bool Active { get; set; } = true;
}