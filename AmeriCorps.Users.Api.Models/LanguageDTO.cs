namespace AmeriCorps.Users.Api.Models;

public sealed class LanguageDTO
{
    public int Id { get; set; }
    public string PickListId { get; set; } = string.Empty;
    public bool IsPrimary { get; set; } 
    public string SpeakingAbility { get; set; } = string.Empty;
    public string WritingAbility { get; set; } = string.Empty;
}