

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AmeriCorps.Users.Data.Core;

public sealed class UserList : Entity 
{
    public string OrgName {get; set; } = string.Empty;

    public List<User> Roles { get; set; } = new List<User>();


}
