namespace AmeriCorps.Users.Models;

public sealed class ExistingUserSearchModel
{
    public string UserEmail { get; set; } = string.Empty;

    public UserRequestModel NewUser { get; set; } = new UserRequestModel();
}