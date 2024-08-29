using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AmeriCorps.Users.Data.Core;

public sealed class Organization : Entity
{
    public required string  OrgName { get; set; }

    public string OrgCode { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;


}