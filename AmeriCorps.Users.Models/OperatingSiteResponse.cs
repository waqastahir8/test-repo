using System.ComponentModel.DataAnnotations;

namespace AmeriCorps.Users.Models;

public sealed class OperatingSiteResponse : OperatingSiteRequestModel
{
    public int Id { get; set; }
}