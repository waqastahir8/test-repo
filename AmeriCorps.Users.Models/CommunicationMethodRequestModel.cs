namespace AmeriCorps.Users.Models;

public sealed class CommunicationMethodRequestModel {

    public int Id { get; set; }
    public required string Type { get; set; }
    public required string Value { get; set; }
    public required bool IsPreferred { get; set; }
}
