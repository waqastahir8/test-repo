using Microsoft.EntityFrameworkCore;

namespace AmeriCorps.Users.Data.Core;

[Index(nameof(RoleName))]
public sealed class Role : Entity
{
    public string RoleName { get; set; } = string.Empty;

    public string FunctionalName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string RoleType { get; set; } = string.Empty;

}