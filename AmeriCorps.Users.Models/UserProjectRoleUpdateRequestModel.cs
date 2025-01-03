namespace AmeriCorps.Users.Models;

public sealed class UserProjectRoleUpdateRequestModel
{
    public int Id { get; set; }

    public AccountStatusRequestModel UserAccountStatus { get; set; }

    public List<UserRoleRequestModel> UserRoles { get; set; } = new List<UserRoleRequestModel>();

    public List<UserProjectRequestModel> UserProjects { get; set; } = new List<UserProjectRequestModel>();
}