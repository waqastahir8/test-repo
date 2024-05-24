namespace AmeriCorps.Users.Data.Core;

public class Attribute : EntityWithUserId
{
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}