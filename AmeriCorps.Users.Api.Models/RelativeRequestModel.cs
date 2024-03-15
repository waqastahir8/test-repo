namespace AmeriCorps.Users.Api.Models;

public sealed class RelativeRequestModel
{
    public int Id { get; set; }
    public required string Relationship { get; set; } = string.Empty;
    public required string HighestEducationLevel { get; set; } = string.Empty;
    public int AnnualIncome { get; set;}
}