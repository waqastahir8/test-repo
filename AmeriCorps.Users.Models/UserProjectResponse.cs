using System.ComponentModel.DataAnnotations;

namespace AmeriCorps.Users.Models;

public sealed class UserProjectResponse : UserProjectRequestModel
{
    public int Id { get; set; }
}