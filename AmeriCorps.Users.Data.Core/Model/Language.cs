namespace AmeriCorps.Users.Data.Core;

public sealed class Language
{
    public int Id { get; set; }
    public string PickListId { get; set; }
    public bool IsPrimary { get; set; }
    public int SpeakingAbility { get; set; }
    public int WritingAbility { get; set; }

    public Language(string pickListId, bool isPrimary, int speakingAbility, int writingAbility){
        PickListId = pickListId;
        IsPrimary = isPrimary;
        SpeakingAbility = speakingAbility;
        WritingAbility = writingAbility;
    }
}
