namespace AmeriCorps.Users.Data.Core;

public sealed class Skill : Entity
{
    public int UserId { get; set; }
    public required string PickListId { get; set; }
}