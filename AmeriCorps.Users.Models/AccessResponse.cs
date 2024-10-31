using System.ComponentModel.DataAnnotations;

namespace AmeriCorps.Users.Models;

public sealed class AccessResponse : AccessRequestModel
{
    public int Id { get; set; }
}