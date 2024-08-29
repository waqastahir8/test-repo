using System.ComponentModel.DataAnnotations;

namespace AmeriCorps.Users.Models;

public sealed class OrganizationResponse : OrganizationRequestModel
{
    public int Id { get; set; }
}