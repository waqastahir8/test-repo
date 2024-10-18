namespace AmeriCorps.Users.Models;

public sealed class ProjectResponse : ProjectRequestModel
{
    public int Id { get; set; }

    public UserResponse? AuthorizedRep { get; set; }
    public UserResponse? ProjectDirector { get; set; }
    public AwardResponse? Award { get; set; }
}