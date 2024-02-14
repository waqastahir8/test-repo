namespace AmeriCorps.Users.Data.Core;
public sealed class BasicInfo {
    public required string UserName { get; set; }
    public string? Prefix { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string? PreferredName { get; set; }
    public required DateTime DateOfBirth { get; set; } 
}