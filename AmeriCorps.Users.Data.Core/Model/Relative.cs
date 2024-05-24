namespace AmeriCorps.Users.Data.Core;

public sealed class Relative : EntityWithUserId
{
    public required string Relationship { get; set; }
    public required string HighestEducationLevel { get; set; }
    public int AnnualIncome { get; set; }
}