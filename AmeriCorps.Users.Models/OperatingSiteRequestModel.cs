namespace AmeriCorps.Users.Models;

public class OperatingSiteRequestModel
{
    public int Id { get; set; }
    public string ProjectCode { get; set; } = string.Empty;
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

    public int InviteUserId { get; set; }
    public DateTime? InviteDate { get; set; }
}