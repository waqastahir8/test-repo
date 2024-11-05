using Microsoft.EntityFrameworkCore;

namespace AmeriCorps.Users.Data.Core;

[Index(nameof(AccessName))]
public sealed class Access : Entity
{
    public string AccessName { get; set; } = string.Empty;
    public int AccessLevel { get; set; }
    public string AccessType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}