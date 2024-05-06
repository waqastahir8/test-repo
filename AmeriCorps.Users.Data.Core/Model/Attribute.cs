namespace AmeriCorps.Users.Data.Core;

public class Attribute : Entity
{

    public int UserId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}