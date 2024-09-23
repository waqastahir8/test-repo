namespace AmeriCorps.Users.Models;

public sealed class UserProjectRoleUpdateRequestModel {

    public int Id { get; set; }

    public string AccountStatus { get; set; } = string.Empty;

    public List<UserRoleRequestModel> UserRoles { get; set; } = new List<UserRoleRequestModel>();

    public List<UserProjectRequestModel> UserProjects { get; set; } = new List<UserProjectRequestModel>();

}