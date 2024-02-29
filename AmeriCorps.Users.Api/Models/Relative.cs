namespace AmeriCorps.Users.Api;

public sealed class Relative
{
    public int Id { get; set; }
    public required string Relationship { get; set; } = string.Empty;
    public required string HighestEducationLevel { get; set; } = string.Empty;
    public int AnnualIncome { get; set;}
}