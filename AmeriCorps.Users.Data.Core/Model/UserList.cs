namespace AmeriCorps.Users.Data.Core;

public sealed class UserList
{
    public string OrgCode { get; set; } = string.Empty;

    public List<User> Users { get; set; } = [];
}
