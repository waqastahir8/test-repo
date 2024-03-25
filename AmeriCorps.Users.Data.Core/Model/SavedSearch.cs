﻿namespace AmeriCorps.Users.Data.Core;

public sealed class SavedSearch
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required string Name { get; set; } = string.Empty;
    public required string Filters { get; set; } = string.Empty;
    public bool NotificationsOn { get; set; }
    public DateTime LastRun { get; set; }
}