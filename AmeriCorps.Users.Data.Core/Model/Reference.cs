namespace AmeriCorps.Users.Data.Core;

public sealed class Reference : Entity
{
    public int UserId { get; set; }
    public required string TypeId { get; set; }
    public required string Relationship { get; set; }
    public required int RelationshipLength { get; set; }
    public required string ContactName { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string Address { get; set; }
    public string Company { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public bool CanContact { get; set; }
    public bool Contacted { get; set; }
    public DateOnly DateContacted { get; set; }
}