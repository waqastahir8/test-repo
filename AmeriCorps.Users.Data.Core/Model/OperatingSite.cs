using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AmeriCorps.Users.Data.Core;

[Index(nameof(OperatingSiteName))]
public sealed class OperatingSite : Entity
{
    public string ProgramYear { get; set; } = string.Empty;
    public bool Active { get; set; } = true;

    public string OperatingSiteName { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string StreetAddress { get; set; } = string.Empty;
    public string StreetAddress2 { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Plus4 { get; set; } = string.Empty;

    public DateTime? InviteDate { get; set; }
    public int InviteUserId { get; set; }
    public DateTime? UpdatedDate { get; set; }
}