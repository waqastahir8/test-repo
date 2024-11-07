namespace AmeriCorps.Users.Models;

public sealed class LanguageRequestModel
{
    public int Id { get; set; }
    public string PickListId { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
    public string SpeakingAbility { get; set; } = string.Empty;
    public string WritingAbility { get; set; } = string.Empty;
}