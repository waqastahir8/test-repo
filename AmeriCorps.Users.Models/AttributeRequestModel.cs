namespace AmeriCorps.Users.Models;

public sealed class AttributeRequestModel
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}