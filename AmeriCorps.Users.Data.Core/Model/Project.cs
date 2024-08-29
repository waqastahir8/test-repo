using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AmeriCorps.Users.Data.Core;

public sealed class Project : Entity
{
    public string ProjectName { get; set; }  = string.Empty;
    public string ProjectCode { get; set; } = string.Empty;
    public string ProjectType { get; set; } = string.Empty;
    public string ProjectOrg { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}