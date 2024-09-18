using System.ComponentModel.DataAnnotations;

namespace AmeriCorps.Users.Models;

public sealed class UserRoleResponse : UserRoleRequestModel
{
    public int Id { get; set; }
}