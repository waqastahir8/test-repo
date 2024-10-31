using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AmeriCorps.Users.Data.Core;

public sealed class ProjectAccess : EntityWithUserProjectId
{
    public string AccessName { get; set; } = string.Empty;

    public int AccessLevel { get; set; }
}