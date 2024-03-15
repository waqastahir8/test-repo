namespace AmeriCorps.Users.Api.Models;

public sealed class SkillRequestModel
{
    public int Id { get; set; }
    public required string PickListId { get; set; }
}