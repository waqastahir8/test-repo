

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AmeriCorps.Users.Data.Core;

public sealed class UserList 
{
    public string OrgCode {get; set; } = string.Empty;

    public List<User> Users { get; set; } = new List<User>();

}
