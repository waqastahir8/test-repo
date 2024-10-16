namespace AmeriCorps.Users.Models;

public class AwardRequestModel
{
    public string AwardCode { get; set; } = string.Empty;
    public string AwardName { get; set; } = string.Empty;

    public string GspListingNumber { get; set; } = string.Empty;
    public int Fain { get; set; }
    public int Uei { get; set; }

    public DateOnly? PerformanceStartDt { get; set; }
    public DateOnly? PerformanceEndDt { get; set; }
}