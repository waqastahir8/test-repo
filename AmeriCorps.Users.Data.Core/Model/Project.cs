using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AmeriCorps.Users.Data.Core;

[Index(nameof(ProjectCode))]
public sealed class Project : Entity
{
    public string ProjectName { get; set; }  = string.Empty;
    public string ProjectCode { get; set; } = string.Empty;
    public string ProjectType { get; set; } = string.Empty;
    public string ProjectOrg { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty; 
    public List<OperatingSite> OperatingSites { get; set; } = new List<OperatingSite>();
}