namespace AmeriCorps.Users.Models;

public class SubGranteeRequestModel
{
    public string GranteeCode { get; set; } = string.Empty;
    public string GranteeName { get; set; } = string.Empty;

    public int Uei { get; set; }

    public string StreetAddress { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public double AwardedMsys { get; set; }

    public double LivingAllowanceMsys { get; set; }
    public double NonLivingAllowanceMsys { get; set; }
}