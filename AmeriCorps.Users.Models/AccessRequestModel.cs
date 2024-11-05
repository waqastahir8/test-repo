namespace AmeriCorps.Users.Models;

public class AccessRequestModel
{
    public string AccessName { get; set; } = string.Empty;
    public int AccessLevel { get; set; }
    public string AccessType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}