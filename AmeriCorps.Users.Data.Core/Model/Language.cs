namespace AmeriCorps.Users.Data.Core;

public sealed class Language : Entity
{
    public int UserId { get; set; }
    public string PickListId { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
    public string SpeakingAbility { get; set; } = string.Empty;
    public string WritingAbility { get; set; } = string.Empty;
}