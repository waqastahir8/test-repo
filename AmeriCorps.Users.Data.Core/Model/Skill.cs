namespace AmeriCorps.Users.Data.Core;

public sealed class Skill : EntityWithUserId
{
    public required string PickListId { get; set; }
}