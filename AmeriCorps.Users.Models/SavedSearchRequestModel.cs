namespace AmeriCorps.Users.Models;

public class SavedSearchRequestModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required string Name { get; set; } = string.Empty;
    public required string Filters { get; set; } = string.Empty;
    public bool NotificationsOn { get; set; }
}