namespace AmeriCorps.Users.Data.Core;
public sealed class CommunicationMethod : EntityWithUserId
{
    public required string Type { get; set; }
    public required string Value { get; set; }
    public required bool IsPreferred { get; set; }
}