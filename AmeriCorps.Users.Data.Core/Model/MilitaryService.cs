namespace AmeriCorps.Users.Data.Core;

public sealed class MilitaryService : Entity
{
    public int UserId { get; set; }
    public required string PickListId { get; set; }
}
