namespace AmeriCorps.Users.Data.Core;

public sealed class Relative : Entity
{
    public int UserId { get; set; }
    public required string Relationship { get; set; }
    public required string HighestEducationLevel { get; set; }
    public int AnnualIncome { get; set; }
}