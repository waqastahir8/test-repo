using Microsoft.EntityFrameworkCore;

namespace AmeriCorps.Users.Data.Core;

[Index(nameof(OrgCode))]
public sealed class Organization : Entity
{
    public required string OrgName { get; set; }

    public string OrgCode { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

}