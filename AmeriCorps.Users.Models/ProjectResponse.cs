using System.ComponentModel.DataAnnotations;

namespace AmeriCorps.Users.Models;

public sealed class ProjectResponse : ProjectRequestModel
{
    public int Id { get; set; }
}