using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AmeriCorps.Users.Data.Core;

public sealed class Access : Entity
{
    public string AccessName { get; set; }  = string.Empty;
    public int AccessLevel { get; set; }
    public string AccessType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}