using System.ComponentModel.DataAnnotations;

namespace AmeriCorps.Users.Api.Models;

public sealed class UserResponse : UserRequestModel
{
    public int Id { get; set; }
}