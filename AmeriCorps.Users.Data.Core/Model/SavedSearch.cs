namespace AmeriCorps.Users.Data.Core;

public sealed class SavedSearch : EntityWithUserId
{
    public required string Name { get; set; } = string.Empty;
    public required string Filters { get; set; } = string.Empty;
    public bool NotificationsOn { get; set; }
    public DateTime LastRun { get; set; }
}