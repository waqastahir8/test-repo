namespace AmeriCorps.Users.Api.Models;

public sealed class AttributeDTO
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}