namespace AmeriCorps.Users.Models;

public sealed class OperatingSiteResponse
{
    public int Id { get; set; }
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

    public UserResponse? Contact { get; set; }

    public double AwardedMsys { get; set; }

    public double LivingAllowanceMsys { get; set; }

    public double NonLivingAllowanceMsys { get; set; }
}