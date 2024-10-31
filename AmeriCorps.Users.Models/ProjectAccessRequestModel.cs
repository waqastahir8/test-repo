namespace AmeriCorps.Users.Models;


public sealed class ProjectAccessRequestModel
{
    // public int Id { get; set; }
    
    public string AccessName { get; set; } = string.Empty;

    public int AccessLevel { get; set; }

}