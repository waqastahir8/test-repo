namespace AmeriCorps.Users.Data.Core;
public sealed class CommunicationMethod {

    public int Id { get; set; }
    public required string Type { get; set; }
    public required string Value { get; set; }
    public required bool IsPreferred { get; set; }
}