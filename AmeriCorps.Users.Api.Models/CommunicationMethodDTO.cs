namespace AmeriCorps.Users.Api.Models;

public sealed class CommunicationMethodDTO {

    public int Id { get; set; }
    public required string Type { get; set; }
    public required string Value { get; set; }
    public required bool IsPreferred { get; set; }
}
