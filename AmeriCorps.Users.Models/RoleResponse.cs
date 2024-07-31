using System.ComponentModel.DataAnnotations;

namespace AmeriCorps.Users.Models;

public sealed class RoleResponse : RoleRequestModel
{
    public int Id { get; set; }
}