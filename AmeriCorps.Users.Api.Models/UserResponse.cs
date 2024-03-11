using System.ComponentModel.DataAnnotations;

namespace AmeriCorps.Users.Api.Models;

public sealed class UserResponse : UserDTO
{
    public int Id { get; set; }
}