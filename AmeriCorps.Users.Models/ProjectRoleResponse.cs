using System.ComponentModel.DataAnnotations;

namespace AmeriCorps.Users.Models;

public sealed class ProjectRoleResponse : ProjectRoleRequestModel
{
    public int Id { get; set; }
}